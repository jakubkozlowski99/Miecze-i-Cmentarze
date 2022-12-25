INCLUDE globals.ink

{ questState < 2 : -> main |-> main2}

 == main ==
Czołem młodzieńcze! Powiedz mi czego potrzebujesz, może mikstura, magiczny pierścień? Zajrzyj, zobacz co mam na składzie, nie pożałujesz!
    + [Pokaż mi swoje towary.] #action:shop
        -> main
    + [Masz dla mnie jakieś zadanie?] #action:quest_ask
        -> quest_available
    + [Bywaj.]
        -> END

== main2 ==

Czołem młodzieńcze! Powiedz mi czego potrzebujesz, może mikstura, magiczny pierścień? Zajrzyj, zobacz co mam na składzie, nie pożałujesz!
    + [Pokaż mi swoje towary.] #action:shop
        -> main
    + [Jeśli chodzi o to zadanie..] #action:quest_ask
        -> quest_started
    + [Bywaj.]
        -> END
-> END

== quest_started ==

{
    - questState == 2:
        Może wrócisz jak je wykonasz, dobrze?
        + [Oczywiście, zabieram się do pracy.]
            -> END
        + [Wróć.]
            -> main2
    - else:
        Fantastycznie! Trzymaj, oto twoja nagroda.
        +[Dziękuję, przyda się.] #action:quest_reward
            -> END
}

== quest_available ==

{
    - questState == 0:
        + [Zrobię to.] #action:quest_give
            -> END
        + [Może innym razem.]
            -> END
    -else:
        Przykro mi, ale nic mi nie przychodzi do głowy.
        + [Wróć.]
            ->END
}