using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AdvancedProgramming.Mvc.Models;
using AdvancedProgramming.Mvc.Models.Chat;

namespace AdvancedProgramming.Mvc.Controllers
{
    public class ChatController : Controller
    {
        // Estado simple del bot en memoria por hilo
        private static readonly ConcurrentDictionary<int, string> BotStage =
            new ConcurrentDictionary<int, string>();

        [HttpGet]
        public ActionResult Widget() => PartialView();

        // Crea o reutiliza hilo priorizando identidad desde Session
        [HttpPost]
        public async Task<ActionResult> OpenOrCreate(string clientName, string clientEmail)
        {
            var sesName = Session["UsuarioNombre"] as string;
            var sesEmail = Session["UsuarioEmail"] as string;

            var finalName = !string.IsNullOrWhiteSpace(sesName) ? sesName :
                             !string.IsNullOrWhiteSpace(clientName) ? clientName : "Guest";

            var finalEmail = !string.IsNullOrWhiteSpace(sesEmail) ? sesEmail :
                             !string.IsNullOrWhiteSpace(clientEmail) ? clientEmail : null;

            if (string.IsNullOrWhiteSpace(finalEmail))
                return new HttpStatusCodeResult(400, "clientEmail is required (no Session email and no clientEmail).");

            using (var db = new AppDbContext())
            {
                var thread = await db.ChatThreads
                    .Where(t => t.State == "Open" && t.ClientEmail == finalEmail)
                    .OrderByDescending(t => t.CreatedUtc)
                    .FirstOrDefaultAsync();

                if (thread == null)
                {
                    thread = new ChatThread
                    {
                        ClientName = finalName,
                        ClientEmail = finalEmail,
                        State = "Open",
                        CreatedUtc = DateTime.UtcNow,
                        NeedsAgent = false
                    };
                    db.ChatThreads.Add(thread);
                    await db.SaveChangesAsync();

                    await AddBotMessage(db, thread.ChatThreadId, "Hello! I’m Marty assistant 🤖.");
                    await AddBotMessage(db, thread.ChatThreadId, MainMenu());
                    BotStage[thread.ChatThreadId] = "main";
                }

                return Json(new { ok = true, threadId = thread.ChatThreadId });
            }
        }

        // Inserta mensaje del cliente o del agente
        [HttpPost]
        public async Task<ActionResult> Send(int threadId, string text, string sender = "Client")
        {
            if (threadId <= 0) return new HttpStatusCodeResult(400, "threadId required");
            if (string.IsNullOrWhiteSpace(text)) return new HttpStatusCodeResult(400, "text required");

            using (var db = new AppDbContext())
            {
                var thread = await db.ChatThreads
                    .FirstOrDefaultAsync(t => t.ChatThreadId == threadId && t.State == "Open");
                if (thread == null) return HttpNotFound("Thread not found or closed");

                var msg = new ChatMessage
                {
                    ChatThreadId = threadId,
                    Text = text.Trim(),
                    Sender = string.IsNullOrWhiteSpace(sender) ? "Client" : sender,
                    SentUtc = DateTime.UtcNow
                };
                db.ChatMessages.Add(msg);

                // 🔔 Si escribe el cliente, marcar alerta para el agente
                if (msg.Sender == "Client")
                    thread.NeedsAgent = true;

                await db.SaveChangesAsync();

                // Si escribe el cliente, el bot responde según menú
                if (msg.Sender == "Client")
                {
                    var reply = await BotReply(threadId, msg.Text, db);
                    if (!string.IsNullOrEmpty(reply))
                        await AddBotMessage(db, threadId, reply);
                }

                // Devuelve messageId para que el widget avance lastId y no duplique
                return Json(new { ok = true, messageId = msg.ChatMessageId, sentUtc = msg.SentUtc });
            }
        }

        // Enviar como agente (atajo) — limpia NeedsAgent
        [HttpPost]
        public async Task<ActionResult> AgentSend(int threadId, string text)
        {
            if (threadId <= 0 || string.IsNullOrWhiteSpace(text))
                return new HttpStatusCodeResult(400, "Invalid params");

            using (var db = new AppDbContext())
            {
                var thread = await db.ChatThreads
                    .FirstOrDefaultAsync(t => t.ChatThreadId == threadId && t.State == "Open");
                if (thread == null) return HttpNotFound();

                db.ChatMessages.Add(new ChatMessage
                {
                    ChatThreadId = threadId,
                    Text = text.Trim(),
                    Sender = "Agent",
                    SentUtc = DateTime.UtcNow
                });

                // ✅ El agente atendió
                thread.NeedsAgent = false;

                await db.SaveChangesAsync();
                return Json(new { ok = true });
            }
        }

        // Historial completo
        [HttpGet]
        public async Task<ActionResult> History(int id)
        {
            using (var db = new AppDbContext())
            {
                var items = await db.ChatMessages
                    .Where(m => m.ChatThreadId == id)
                    .OrderBy(m => m.ChatMessageId) // asegura coherencia con sinceId
                    .Select(m => new { m.ChatMessageId, m.Sender, m.Text, m.SentUtc })
                    .ToListAsync();

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        // Poll incremental (desde el último id que tiene el cliente/agente)
        [HttpGet]
        public async Task<ActionResult> Poll(int id, int sinceId = 0)
        {
            using (var db = new AppDbContext())
            {
                var items = await db.ChatMessages
                    .Where(m => m.ChatThreadId == id && m.ChatMessageId > sinceId)
                    .OrderBy(m => m.ChatMessageId)
                    .Select(m => new { m.ChatMessageId, m.Sender, m.Text, m.SentUtc })
                    .ToListAsync();

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        // Lista de hilos abiertos (para la consola del agente) — prioriza NeedsAgent
        [HttpGet]
        public async Task<ActionResult> OpenThreads()
        {
            using (var db = new AppDbContext())
            {
                var rows = await db.ChatThreads
                    .Where(t => t.State == "Open")
                    .OrderByDescending(t => t.NeedsAgent)
                    .ThenByDescending(t => t.CreatedUtc)
                    .Select(t => new { t.ChatThreadId, t.ClientName, t.ClientEmail, t.CreatedUtc, t.NeedsAgent })
                    .ToListAsync();

                return Json(rows, JsonRequestBehavior.AllowGet);
            }
        }

        // 🔔 Endpoint para alertas del agente (badge/sonido)
        [HttpGet]
        public async Task<ActionResult> AgentAlerts()
        {
            using (var db = new AppDbContext())
            {
                var rows = await db.ChatThreads
                    .Where(t => t.State == "Open" && t.NeedsAgent)
                    .OrderByDescending(t => t.CreatedUtc)
                    .Select(t => new { t.ChatThreadId, t.ClientName, t.ClientEmail, t.CreatedUtc })
                    .ToListAsync();

                return Json(rows, JsonRequestBehavior.AllowGet);
            }
        }

        // Cerrar hilo
        [HttpPost]
        public async Task<ActionResult> Close(int id)
        {
            using (var db = new AppDbContext())
            {
                var th = await db.ChatThreads.FindAsync(id);
                if (th == null) return HttpNotFound();
                th.State = "Closed";
                th.ClosedUtc = DateTime.UtcNow;
                th.NeedsAgent = false;
                await db.SaveChangesAsync();
                return Json(new { ok = true });
            }
        }

        // ---------- Bot helpers ----------
        private static string MainMenu() =>
            "Please choose an option:\n" +
            "1) Plans & pricing\n" +
            "2) Documentation\n" +
            "3) Business hours & contact\n" +
            "9) Talk to a human agent\n" +
            "0) Back to menu";

        private async Task<string> BotReply(int threadId, string input, AppDbContext db)
        {
            var q = (input ?? "").Trim().ToLowerInvariant();
            BotStage.TryGetValue(threadId, out var stage);
            stage = stage ?? "main";

            if (q == "0")
            {
                BotStage[threadId] = "main";
                return MainMenu();
            }

            if (q == "9" || q.Contains("agent"))
            {
                BotStage[threadId] = "handoff";

                // 🔔 marca el hilo para atención humana
                var thread = await db.ChatThreads.FindAsync(threadId);
                if (thread != null)
                {
                    thread.NeedsAgent = true;
                    await db.SaveChangesAsync();
                }

                return "Okay! I’ll notify an agent. Please hold on...";
            }

            if (stage == "main")
            {
                if (q == "1" || q.Contains("price") || q.Contains("plan"))
                    return "We offer Basic ($29), Pro ($79), and Enterprise (custom).\nType 9 for an agent or 0 to go back.";
                if (q == "2" || q.Contains("doc"))
                    return "Docs: go to Help → Documentation in the top menu.\nType 9 for an agent or 0 to go back.";
                if (q == "3" || q.Contains("contact") || q.Contains("phone") || q.Contains("email"))
                    return "We’re online Mon–Fri, 9am–6pm (GMT-6).\nEmail: support@example.com | Phone: +506 0000-0000.\nType 9 for an agent or 0 to go back.";

                return "I didn’t get that.\n" + MainMenu();
            }

            return null;
        }
        s
        private static async Task AddBotMessage(AppDbContext db, int threadId, string text)
        {
            var bot = new ChatMessage
            {
                ChatThreadId = threadId,
                Text = text,
                Sender = "Bot",
                SentUtc = DateTime.UtcNow
            };
            db.ChatMessages.Add(bot);
            await db.SaveChangesAsync();
        }
    }
}
