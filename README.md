# 🔥 FireBlaze

**FireBlaze** è un'applicazione web sviluppata in **Blazor Server (.NET 9)** che integra:

- **Firebase Storage** per il caricamento e la lettura di file remoti
- **Visualizzazione di modelli 3D (.glb)** direttamente nel browser tramite la libreria **Blazor3D**
- Un'interfaccia semplice per l'elenco e la gestione dei file archiviati in Firebase

---

## ✨ Funzionalità principali

- ✅ Upload di file su **Firebase Storage**
- ✅ Lettura e visualizzazione dell'elenco file da un bucket specifico
- ✅ Visualizzazione 3D di modelli `.glb` caricati nella cartella `wwwroot/models/`
- ✅ Architettura modulare e pronta per test/fake service
- ✅ Rendering interattivo tramite `@rendermode InteractiveServer`

---

## 🛠️ Tecnologie utilizzate

| Tecnologia | Descrizione |
|-----------|-------------|
| [Firebase Storage](https://firebase.google.com/docs/storage) | Archiviazione file cloud |
| [Blazor3D](https://github.com/HomagGroup/Blazor3D) | Visualizzazione modelli 3D (Three.js wrapper in Blazor) |
