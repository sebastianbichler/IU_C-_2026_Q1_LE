UML Klassendiagramm - Rollenspiel

'''
classDiagram
    %% --- Abstrakte Basisklassen & Interfaces ---
    class Entity {
        <<Abstract>>
        #String name
        #int healthPoints
        #int maxDamage
        +takeDamage() void
        +isDead() bool
    }

    class IAttacker {
        <<Interface>>
        +attack(Entity target) void
    }

    class Object {
        <<Abstract>>
        #string name
        #bool isConsumeable
    }

    %% --- Haupt-Charakterklassen ---
    class Hero {
        +attack(Entity target) void
        +healing() void
    }

    class Monster {
        +attack(Entity target) void
        +healing() void
    }

    %% --- Hero Unterklassen ---
    class Swordsman {
        +attack(Entity target) void
        +healing() void
    }

    class Mage {
        +attack(Entity target) void
        +healing() void
    }

    %% --- Monster Unterklassen ---
    class Orc {
        +attack(Entity target) void
    }

    class Dragon {
        +attack(Entity target) void
    }

    %% --- Gegenstände ---
    class Potion {
        #bool hasBadeffect
        +usePotion() void
    }

    class Chest {
        #bool isMimic
        +open() void
    }

    %% --- Beziehungen ---
    
    %% Vererbung (Inheritance) - Durchgezogene Linie mit Pfeil
    Entity <|-- Hero
    Entity <|-- Monster
    
    Hero <|-- Swordsman
    Hero <|-- Mage
    
    Monster <|-- Orc
    Monster <|-- Dragon
    
    Object <|-- Potion
    Object <|-- Chest

    %% Implementierung (Realization) - Gestrichelte Linie mit Pfeil
    IAttacker <|.. Hero
    IAttacker <|.. Monster
'''
