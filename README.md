# VR Physic Garden 2

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
- NerverSoft JSON

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
- Il ramo main è bloccato. Se vuoi proponi le modifiche con una pull request, che verrà valutata dall'autore.
- Le impostazioni dei moduli fisici sono configurabili dall'Inspector di Unity.

## Autore

Riccardo Tulini

## Licenza

Distribuito con una licenza MIT modificata. Vedi il file [LICENSE](LICENSE).
