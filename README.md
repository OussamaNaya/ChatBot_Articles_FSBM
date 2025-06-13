# 🧠 Application Chatbot – Récupération d'Articles PDF d'un Professeur

## 📌 Description

Cette application web permet aux étudiants de **contacter un chatbot intelligent** pour récupérer automatiquement **tous les articles scientifiques** d'un professeur, sous **format PDF**.  
Elle propose également une **authentification sécurisée**, un espace personnel, des **statistiques d'usage**, et une interface conversationnelle intuitive.

---

## 🛠️ Technologies utilisées

- **Frontend** : HTML, CSS, JavaScript  
- **Backend** : ASP.NET Core MVC  
- **Base de données** : SQL Server  
- **IA / Chatbot** : [LLama 3](https://together.ai/models/meta-llama/Llama-3-8B-Instruct) via l'API de **Together.ai**

---

## 🎯 Fonctionnalités

- 🔐 Page **Connexion** et **Inscription** sécurisée
![image](https://github.com/user-attachments/assets/cf1034d1-4b09-4709-b2a3-751be37b28a7)
---
![image](https://github.com/user-attachments/assets/04dce083-7cf6-4250-be50-73b44b787fb9)

- 💬 Interface **Chatbot** intelligente
![image](https://github.com/user-attachments/assets/23a420d6-c656-48eb-8919-d26c495e56da)

- 📄 Téléchargement des **articles PDF**
![image](https://github.com/user-attachments/assets/e829d341-bef8-4a71-991b-7e0e8271458b)

- 📊 Statistiques d'utilisation (articles téléchargés, historique, etc.)
![image](https://github.com/user-attachments/assets/e4453d90-e532-49df-a319-a0e426f2e6aa)

- ⚙️ Intégration API avec gestion de prompt LLama 3
![image](https://github.com/user-attachments/assets/e5eb36e1-addd-4597-83ed-97b50f85f454)


---

## 🚀 Lancer le projet

1. Cloner le dépôt :
   ```bash
   git clone https://github.com/OussamaNaya/ChatBot_Articles_FSBM.git
   ```

2. Ouvrir le projet dans Visual Studio 2022 ou plus

3. Configurer la connexion SQL Server dans `appsettings.json`

4. Ajouter votre clé API de Together.ai :
   ```json
   "TogetherAI": {
     "ApiKey": "VOTRE_CLE_API"
   }
   ```

5. Lancer le projet :
   ```bash
   dotnet run
   ```

---


## 👤 Auteur

**NAYA OUSSAMA**  
Étudiante en Master Big Data & Data Science

- 📫 Email : oussamanaya8@gmail.com
- 💼 LinkedIn : [linkedin.com/in/NAYA OUSSAMA](https://www.linkedin.com/in/oussama-naya-a4a398249/)

---

## 📝 Licence

Ce projet est sous licence MIT – voir le fichier LICENSE pour plus d'informations.
