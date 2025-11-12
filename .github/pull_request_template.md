- [ ] Navngivning
    - Klasser
    - Records
    - Interface
    - Properties
    - Fields
    - Metodenavne
    - Metodeparametre
    - Primære constructors på klasser
    - Primære constructors på records

- [ ] Placering
    - Entiteter
    - Repository interfaces
    - Repository implementeringer
    - CommandHandler interfaces
    - CommandHandler implementeringer
    - QueryHandler interfaces
    - QueryHandler implementeringer
    - DbContext
    - Commands
    - Queries

- [ ] Blazor
    - Er lokale (bruges kun i en enkelt komponent) css klasser i komponentens egen css fil?
    - Anvendes globale css klasser til alle komponenter som skal se ens ud?
     
- [ ] Exceptions
    - Anvendes klasse-specifikke exceptions?
    - Fanges alle exceptions i præsentationslaget?

- [ ] Er koden kommenteret?
    - Hvor koden ikke er gennemskuelig
    - Hvor kodebeslutningen skal begrundes

- [ ] Er der skrevet summaries til typer og metoder?
- [ ] Er der skrevet ADR'er på væsentlige beslutninger?
- [ ] Skal der skrives resultatafsnit over features i denne PR?
- [ ] Er metoder som skal være public public, og private private?
- [ ] Er properties og fields som skal være nullable nullable?
- [ ] Fanges alle exceptions?

#### Domain
- [ ] Invarianter
- [ ] Er property getters og setters sat korrekt
    - private set på klasser
    - private init på records

- [ ] Er domæne model opdateret med ændringer til entiteter?
