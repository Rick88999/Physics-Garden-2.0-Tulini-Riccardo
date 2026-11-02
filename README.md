# VR Physics Garden 2

🇮🇹 [Italiano](#-italiano) | 🇬🇧 [English](#-english)

---

# 🇮🇹 Italiano

Gioco educativo in realtà virtuale per l'apprendimento e/o il ripasso della fisica, sviluppato in Unity
come progetto di tesi triennale in Ingegneria dell'Informazione (Informatica), Sapienza Università di Roma.

Il giocatore esplora un laboratorio virtuale e interagisce con simulazioni fisiche in prima persona, risolvendo i vari esperimenti.

## Funzionalità

- **Rotolamento puro**: sfere e cilindri su un piano inclinato regolabile, con attrito personalizzato e rilevamento della normale alla superficie tramite raycast.
- **Mongolfiera**: simulazione con principio di Archimede e smorzamento dinamico, con oggetti trasportabili nel cesto.
- **Bilancia**: meccaniche di interazione fisica con oggetti e masse.
- **Terminale interattivo**: formule renderizzate con TextMeshPro e validazione delle risposte con tolleranza legata all'errore della simulazione PhysX.
- **Supporto multi-visore**: rilevamento automatico del tipo di controller e gestione di luci, audio e bloom dalle impostazioni VR.

## Piattaforme supportate

- Meta Quest 3
- HTC Vive CE

## Requisiti

- Unity `(LTS)` `6000.3.11f1` con Universal Render Pipeline (URP)
- XR Interaction Toolkit
- Visore compatibile (vedi sopra)
- Newtonsoft JSON

## Come avviare il progetto

1. Clona il repository:
   ```bash
   git clone [URL-DEL-REPOSITORY]
   ```
2. Apri la cartella con Unity Hub, usando la versione indicata sopra.
3. Apri la scena principale `SampleScene`.
4. Collega il visore (Link/USB) e premi Play.

## Controlli

| Visore | Movimento | Azioni |
|---|---|---|
| Meta Quest 3 | Stick del controller | Grip per afferrare, pulsanti A/B/X/Y |
| HTC Vive CE | Trackpad sinistro | Trackpad destro emula A/B/X/Y |

## Struttura del progetto

```
Assets/
  Scripts/     # logica delle simulazioni e interazione VR
  Scenes/      # scene del gioco
  Models/      # modelli 3D
```

## Per chi vuole estendere il progetto, ai futuri tesisti

Il codice è pensato per essere ampliato da futuri tesisti.

- Fai un fork del repository e lavora sul tuo ramo, al fine di continuare la tesi
  (suggerisco nel nome del ramo di aggiungere il nome del tesista o almeno l'anno di tesi).
- Il ramo main è bloccato. Se vuoi, proponi le modifiche con una pull request, che verrà valutata dall'autore.
- Le impostazioni dei moduli fisici sono configurabili dall'Inspector di Unity.

## Autore

Riccardo Tulini

## Licenza

Distribuito con una licenza MIT. Vedi il file [LICENSE](LICENSE).

---

# 🇬🇧 English

An educational virtual reality game for learning and/or reviewing physics, developed in Unity
as a bachelor's thesis project in Information Engineering (Computer Science track), Sapienza University of Rome.

The player explores a virtual laboratory and interacts with physics simulations in first person, solving the various experiments.

## Features

- **Pure rolling**: spheres and cylinders on an adjustable inclined plane, with custom friction and surface normal detection via raycast.
- **Hot air balloon**: simulation based on Archimedes' principle and dynamic damping, with objects that can be carried in the basket.
- **Scale**: physical interaction mechanics with objects and masses.
- **Interactive terminal**: formulas rendered with TextMeshPro and answer validation with a tolerance tied to the PhysX simulation error.
- **Multi-headset support**: automatic controller type detection and management of lighting, audio and bloom from the VR settings.

## Supported platforms

- Meta Quest 3
- HTC Vive CE

## Requirements

- Unity `(LTS)` `6000.3.11f1` with Universal Render Pipeline (URP)
- XR Interaction Toolkit
- Compatible headset (see above)
- Newtonsoft JSON

## Getting started

1. Clone the repository:
   ```bash
   git clone [REPOSITORY-URL]
   ```
2. Open the folder with Unity Hub, using the version indicated above.
3. Open the main scene `SampleScene`.
4. Connect the headset (Link/USB) and press Play.

## Controls

| Headset | Movement | Actions |
|---|---|---|
| Meta Quest 3 | Controller stick | Grip to grab, A/B/X/Y buttons |
| HTC Vive CE | Left trackpad | Right trackpad emulates A/B/X/Y |

## Project structure

```
Assets/
  Scripts/     # simulation logic and VR interaction
  Scenes/      # game scenes
  Models/      # 3D models
```

## For those who want to extend the project, future thesis students

The code is designed to be extended by future thesis students.

- Fork the repository and work on your own branch in order to continue the thesis
  (I suggest adding the student's name, or at least the thesis year, to the branch name).
- The main branch is locked. If you wish, propose your changes through a pull request, which will be reviewed by the author.
- The settings of the physics modules can be configured from the Unity Inspector.

## Author

Riccardo Tulini

## License

Distributed under a MIT license. See the [LICENSE](LICENSE) file.
