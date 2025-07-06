# 📊 aestus-app

**Aestus** je mini sustav za detekciju sumnjivih transakcija po korisniku.  
Sustav se sastoji od .NET 8 backend REST API-ja i Angular frontend aplikacije.  
Namijenjen je testiranju heurističke detekcije anomalija u financijskim tokovima.

---

## 🚀 Pokretanje aplikacije

### ✅ 1. Backend (aestusApi)

#### 📋 Preduvjeti:
- .NET 8 SDK
- SQL Server (lokalni ili Azure)

#### ▶️ Pokretanje:

```bash
cd aestusApi
dotnet restore
dotnet ef database update
dotnet run
```

#### 🌱 Seed podaci:
Prilikom pokretanja, aplikacija automatski puni bazu testnim transakcijama ako je prazna.

#### 📚 API dokumentacija:
Dostupna na:  
👉 https://localhost:7156/swagger

---

### ✅ 2. Frontend (anomaly)

#### 📋 Preduvjeti:
- Node.js 18+
- Angular CLI (`npm install -g @angular/cli`)

#### ▶️ Pokretanje:

```bash
cd anomaly
npm install
ng serve
```

Frontend se pokreće na:  
👉 http://localhost:4200

#### 🔒 Napomena:
Aplikacija je trenutno **zakucana za `userId = 1`**.  
Svi zahtjevi i prikazi vezani su za transakcije ovog korisnika.

---

## 🛠️ Tehnologije

- .NET 8 (Web API)
- Angular 20
- SQL Server
- Angular Material
- ApexCharts

---

## 🧪 Testiranje

- Backend podržava ≥ 1000 transakcija/s
- Sadrži osnovne seed podatke i testne scenarije

---


