// Defines the valid keys that identify which content section should be displayed
export namespace InformativeSectionTypes {
    export type ContentKey =
        | "info"
        | "howItWorks"
        | "security"
        | "privacy"
        | "mainFunctions"
        | "compatibility"
        | "contacts";
}

// Provides the logic for updating the displayed content based on the selected section
export class InformativeSectionActions 
{

    // Maps each content key to its corresponding title and text
    private static contentList = {
        info: { 
            title: "Cos'è gestore password?", 
            text: 
                `
                <p style="display: inline-block; font-weight: 600;">Gestore password</p>

                <p style="display: inline-block; margin-top: -5px;">
                    è un'applicazione desktop nata con l'obiettivo di semplificare la gestione della propria identità digitale.
                    In un mondo in cui utilizziamo decine di account diversi, ricordarne ogni password è impossibile, mentre usarne una sola per tutto è pericoloso.
                </p>

                <ul style="list-style-type: '● '; margin-top: 0px;">
                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 5px;">A chi è rivolta: </p>

                        <p style="display: inline-block; margin-top: -5px;">
                            A chiunque desideri centralizzare le proprie credenziali in un unico luogo sicuro, eliminando post-it cartacei o file di testo non protetti.
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 5px;">Perché scegliere noi: </p>

                        <p style="display: inline-block; margin-top: -5px;">
                            Gestore password è un'applicazione costantemente aggiornata, un'assistenza che garantisce risposte in 24/48h e sempre nuove funzionalità.
                        </p>
                    </li>
                </ul>
                `
        },
        howItWorks: { 
            title: "Come funziona?", 
            text: 
                `
                <p>L'utilizzo dell'app è intuitivo e immediato: </p>

                <ul style="list-style-type: '● '">
                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 5px;">Salvataggio password: </p>
                        <p style="display: inline-block; margin-top: -5px;">
                            Puoi aggiungere nuovi account specificando app/sito, nome utente e password
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 5px;">Crittografia: </p>

                        <p style="display: inline-block; margin-top: -5px;">
                            Ogni volta che salvi un dato, questo viene trasformato istantaneamente in un codice illeggibile prima di essere memorizzato.
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 5px;">Recupero password: </p>
                        
                        <p style="display: inline-block; margin-top: -5px;">
                            L'accesso al database avviene tramite un'unica Master Password. Solo chi possiede questa chiave può visualizzare i dati in chiaro.
                        </p>
                    </li>
                </ul>
                `
        },
        security: { 
            title: "Sicurezza", 
            text: 
                `
                <p>
                    La sicurezza è il cuore pulsante di Gestore Password. Sono stati implementati alti standard del settore per garantire
                    che i tuoi dati siano inattacabili:
                </p>

                <ul style="list-style-type: '● '">
                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px;">Hashing: </p>

                        <p style="display: inline-block; margin-top: -5px;">
                            La password del tuo account viene protetta tramite Argon2id, un algoritmo moderno progettato per resistere ad attacchi hardware (GPU, ASIC) e software.
                            Ogni password viene hashata con un salt univoco, così da impedire attacchi con rainbow table.
                            Durante la verifica, il confronto degli hash avviene tramite una funzione a tempo costante per prevenire attacchi basati sul timing.
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px;">Crittografia: </p>
                        
                        <p style="display inline-block; margin-top: -5px;"> 
                            Le password delle tue app vengono protette tramite AES‑256.
                            La chiave di cifratura viene derivata dalla tua master password tramite Argon2id, rendendo impossibile ricavarla tramite brute‑force.
                            Ogni voce viene cifrata in modalità CBC con un IV casuale, così lo stesso dato non produce mai lo stesso output.
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px;">Token JWT</p>
                        
                        <p style="display: inline-block; margin-top: -5px;"> 
                            Per tutte le operazioni sensibili relative alla sicurezza dell’account è richiesta l’autenticazione tramite un token JWT.
                            Il server verifica la validità del token ad ogni richiesta, così da garantire che solo l’utente autenticato possa eseguire azioni protette.
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px;">Zero-Knowledge Achitecture: </p>

                        <p style="display: inline-block; margin-top: -5px;">
                            Nessun dato viene inviato a terzi e nessuno può leggere in chiaro le tue password oltre te.
                        </p>
                    </li>
                </ul>
                ` 
        },
        privacy: { 
            title: "Privacy", 
            text: 
                `
                <p>La tua privacy non è un'opzione, è un diritto.</p>

                <ul style="list-style-type: '● '">
                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px">Cosa viene salvato:</p>

                        <p style="display: inline-block; margin-top: -5px;">
                            Solo le credenziali che decidi di salvare.
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px;">Cosa non viene salvato</p>

                        <p style="display: inline-block; margin-top: -5px;">
                            Non tracciamo la tua posizione, non leggiamo la tua cronologia e non abbiamo accesso alla tua Master Password.
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px;">Nessuna condivisione: </p>

                        <p style="display: inline-block; margin-top: -5px;">
                            I tuoi dati appartengono a te. Non vendiamo né condividiamo informazioni con aziende pubblicitarie o terze parti.
                        </p>
                    </li>
                </ul>
                ` 
        },
        mainFunctions: { 
            title: "Funzionalità principali", 
            text: 
                `
                <ul style="list-style-type: '● '">
                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px;">Generatore di password: </p>

                        <p style="display: inline-block; margin-top: -5px;">
                            Crea istantaneamente password complesse (es: f9!GkL29#zP3D.5g) difficili da hackerare con un semplice click.
                        </p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight: 600; margin-top: 0px;">Ricerca rapida: </p>

                        <p style="display: inline-block; margin-top: -5px;">
                            Trova la password dell'acount che ti serve in un istante grazie alla barra di ricerca.
                        </p>
                    </li>
                </ul>
                `
        },
        compatibility: { 
            title: "Compatibilità", 
            text: 
                `
                <p>L'applicazione è progettata per seguirti ovunque: </p>

                <ul style="list-style-type: '● '">
                    <li>
                        <p style="display: inline-block; font-weight; 600; margin-top: 0px;">Windows</p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight; 600; margin-top: 0px;">MacOS</p>
                    </li>

                    <li>
                        <p style="display: inline-block; font-weight; 600; margin-top: 0px;">Linux</p>
                    </li>
                </ul>

                <a href="https://gestorepassword.com/download">Scarica ora!</a>
                ` 
        },
        contacts: { 
            title: "Contatti e supporto", 
            text: 
                `
                <p>
                    Hai riscontrato problemi o hai suggerimenti? 
                </p>

                <p>
                    Mandaci una mail: 
                    <a href="mailto:officialvaultmanager@gmail.com">officialvaultmanager@gmail.com</a>
                </p>
                `
        }
    };

    // Updates the page content based on the selected section key
    public static ChangeMainContent(contentToShow: InformativeSectionTypes.ContentKey) 
    {
        const titleHeader = document.getElementById("contentTitle") as HTMLHeadingElement;
        const textParagraph = document.getElementById("contentText") as HTMLParagraphElement;

        const selected = this.contentList[contentToShow];

        titleHeader.textContent = selected.title;
        textParagraph.innerHTML = selected.text;
    }
}
