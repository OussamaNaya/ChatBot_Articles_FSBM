using System.Diagnostics;
using System.Text.RegularExpressions;
using ChatBot_Articles_Prof_FSBM.Data;
using ChatBot_Articles_Prof_FSBM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ChatBot_Articles_Prof_FSBM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            ViewBag.EstConnecte = (etudiant != null); // Toujours défini

            if (etudiant != null)
            {
                return View(etudiant);
            }
            else
            {
                return View("Connexion");
            }

        }
        public IActionResult Diconnexion()
        {
            // Supprimer l'objet Etudiant de la session
            HttpContext.Session.Remove("Etudiant");

            ViewBag.EstConnecte = false; // Toujours défini

            // Rediriger vers la vue de connexion
            return View("Connexion");
        }

        [HttpGet]
        public IActionResult Inscription()
        {
            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            ViewBag.EstConnecte = (etudiant != null); // Toujours défini

            return View();
        }

        [HttpPost, ActionName("Inscription")]
        public IActionResult InscriptionPost(Etudiant etu)
        {
            var e = _db.Etudiant.FirstOrDefault(e => e.Email == etu.Email);
            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            ViewBag.EstConnecte = (etudiant != null); // Toujours défini


            if (e == null)
            {
                etu.FkRole = 1;
                // Hachage du mot de passe
                var hasher = new PasswordHasher<Etudiant>();
                etu.Pass = hasher.HashPassword(etu, etu.Pass); // Important : remplacer le champ par le hash

                _db.Etudiant.Add(etu);
                _db.SaveChanges();

                TempData["succes"] = "L'étudiant a été créé avec succès.";

                return View("Connexion");
            }
            else
            {
                TempData["erreur"] = "Cette adresse email existe déjà dans la base de données !";

                return View();
            }
        }

        [HttpGet]
        public IActionResult Connexion()
        {
            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            ViewBag.EstConnecte = (etudiant != null); // Toujours défini

            return View();
        }

        [HttpPost, ActionName("Connexion")]
        public IActionResult ConnexionPost(Etudiant etu)
        {
            // Rechercher l'étudiant par email
            var etudiant = _db.Etudiant.FirstOrDefault(e => e.Email == etu.Email);

            if (etudiant != null)
            {
                var hasher = new PasswordHasher<Etudiant>();

                // Vérifier le mot de passe haché
                var result = hasher.VerifyHashedPassword(etudiant, etudiant.Pass, etu.Pass);

                if (result == PasswordVerificationResult.Success)
                {
                    // Connexion réussie
                    TempData["succes"] = "Connexion réussie.";
                    // Ici, tu peux rediriger vers une autre page ou enregistrer une session
                    HttpContext.Session.SetObjectAsJson("Etudiant", etudiant);

                    return RedirectToAction("Index"); // Exemple
                }
            }

            TempData["erreur"] = "Email ou mot de passe incorrect.";
            return View();
        }
        public IActionResult Privacy()
        {
            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            ViewBag.EstConnecte = (etudiant != null); // Toujours défini

            return View("Privacy");
        }

        [HttpGet]
        public JsonResult GetProfesseurs()
        {
            var profs = _db.Prof.ToList();
            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            ViewBag.EstConnecte = (etudiant != null); // Toujours défini

            return Json(profs);
        }

        [HttpGet]
        public JsonResult GetHistorique()
        {
            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            if (etudiant != null)
            {
                var hist = _db.Historique.Where(his => his.FkEtudiant == etudiant.Id).ToList();

                return Json(hist);
            }

            return Json(new { Response = "" });
        }

        [HttpGet]
        public bool AddToHistorique(Etudiant etu, Prof prof, string question)
        {
            if (etu == null || prof == null || string.IsNullOrWhiteSpace(question))
                return false;

            var historique = new Historique
            {
                FkEtudiant = etu.Id,
                FkProf = prof.Id,
                Question = question,
            };

            _db.Historique.Add(historique);
            _db.SaveChanges();

            return true;
        }


        [HttpGet]
        public IActionResult tique()
        {
            var stats = _db.Historique
                           .GroupBy(h => h.Prof.Nom)
                           .Select(g => new ModelProf
                           {
                               NomProf = g.Key,
                               NombreRecherches = g.Count()
                           })
                           .ToList();

            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            ViewBag.EstConnecte = (etudiant != null); // Toujours défini

            return View(stats);
        }

        [HttpPost]
        public JsonResult TraiterMessage([FromBody] ModelMessage message)
        {
            // Traitement du message
            string texte = message.Message;
            string reponse = $"Vous avez envoyé : '{texte}'";

            List<String> articles = new List<String>();

            // 1. Récupérer les professeurs
            List<Prof> profs = _db.Prof.ToList();
            if (profs == null || profs.Count == 0)
            {
                return Json(new { reponse = "Aucun professeur trouvé dans la base." });
            }

            // 2. Générer la liste numérotée des noms
            int i = 1;
            string param = "";
            foreach (var p in profs)
            {
                param += $"{i}. {p.Nom}\n";
                i++;
            }

            // 3. Construire le prompt
            var prompt = $@"
                Tu es un agent spécialisé dans l'analyse de texte. Ta mission est de classifier **précisément** le texte fourni en **exactement l'une** des catégories suivantes :

                {param}

                ---

                **Règles de classification** :
                - Lis attentivement le texte entre guillemets.
                - Identifie les **noms propres** mentionnés, même s'ils sont **mal orthographiés ou partiellement incorrects** (ex. : ""Azmil"" ≈ ""Amzil"").
                - Si le nom dans le texte correspond clairement (ou approximativement) à l’un des noms ci-dessus, retourne ce nom.
                - Si **aucun nom** ne correspond de manière suffisamment claire, retourne **""Inconnue""**.

                ---

                **Format de réponse** : tu dois répondre uniquement avec un objet JSON ayant cette structure :

                ```json
                {{
                  ""raison"": ""Raison de la classification"",
                  ""prof"": ""Le nom du prof parmi la liste, ou 'Inconnue'""
                }}

                --- Texte à classifier : ""{texte}""
                --- Réponse attendue :
            ";

            // 4. Appeler le script Python
            string result;
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = "python", // Assure-toi que "python" est dans le PATH
                    Arguments = $"\"C:\\Users\\LENOVO\\Documents\\ASP.NET\\ChatBot_Articles_Prof_FSBM\\ChatBot_Articles_Prof_FSBM\\Services\\ServiceAI.py\" \"{prompt.Replace("\"", "\\\"")}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(psi))
                {
                    result = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                }

            }
            catch (Exception ex)
            {
                return Json(new { prof = $"Erreur lors de l’appel du service Python: {ex.Message}", Articles = articles });
            }


            string rawResponse = result; // Remplace par ta vraie variable

            // 1. Supprimer le bloc markdown ```json ... ```
            var jsonMatch = Regex.Match(rawResponse, "```json\\s*(\\{.*?\\})\\s*```", RegexOptions.Singleline);
            string profName = "";
            if (jsonMatch.Success)
            {
                string jsonString = jsonMatch.Groups[1].Value;

                // 2. Parser le JSON
                JObject json = JObject.Parse(jsonString);
                profName = json["prof"]?.ToString();

                //Console.WriteLine("Prof détecté : " + prof);

                articles = GetArticlesByProf(profName);
            }
            else
            {
                Console.WriteLine("Format de réponse inattendu.");
            }


            // Ajouter la question dans l'Historique.
            var etudiant = HttpContext.Session.GetObjectFromJson<Etudiant>("Etudiant");
            var prof = _db.Prof.FirstOrDefault(pr => pr.Nom == profName);

            if (etudiant != null && prof != null)
            {
                AddToHistorique(etudiant, prof, texte);
                // ou si tu veux un retour :
                // bool isHistoriqueAdded = AddToHistorique(etudiant, prof, texte);
            }


            return Json(new { prof = profName, articles = articles });
        }

        [HttpPost]
        public List<String> GetArticlesByProf(string profName)
        {
            var prof = _db.Prof.FirstOrDefault(p => p.Nom == profName);

            List<String> articles = new List<String>();

            if (prof != null)
            {
                articles = _db.Article
                            .Where(art => art.FkProf == prof.Id)
                            .Select(art => art.UrlArticle) // ← sélection directe du champ
                            .ToList();

            }

            return articles;
        }


        // Et le modèle
        public class ModelMessage
        {
            public string Message { get; set; }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
