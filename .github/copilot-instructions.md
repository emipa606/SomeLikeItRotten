# GitHub Copilot Instructions for Some Like It Rotten Mod

## Mod Overview and Purpose

**Some Like It Rotten** is a RimWorld mod designed to allow specific creatures to consume rotting and desiccated corpses. The primary purpose is to utilize these corpses as a food source for certain creatures, without causing them food poisoning, thereby solving the problem of corpse disposal in a thematic and balanced manner. Inspired as a companion mod to the "Rats!" mod, it helps players manage corpses through a rat-farm approach.

### Key Features and Systems

- **Eating Rotting Corpses**: This mod allows all creatures to potentially eat rotting and desiccated corpses. However, only the creatures specified in the settings will do so without incurring food poisoning.
- **Nutrition Value Adjustment**: Rotting corpses provide 50% of their original nutrition, while desiccated corpses provide 25%.
- **Compatibility**: It is compatible with saves and adaptable to all creatures/races in the game.

### Coding Patterns and Conventions

- **Static Classes**: Core functionalities are encapsulated within static classes (`Corpse_IngestibleNow`, `FoodUtility_AddFoodPoisoningHediff`, `FoodUtility_TryFindBestFoodSourceFor`, and `SomeLikeItRotten`) for easy accessibility and utility.
- **Internal Classes**: Mod settings and core mod operations are managed by internal classes (`SomeLikeItRottenMod` and `SomeLikeItRottenModSettings`), aligning with typical RimWorld's mod structure.
- **Naming Conventions**: PascalCase is used for class and method names to maintain consistency with C# standards.
- **Inheritance**: The class `StatPart_IsCorpseFreshOrRotten` extends `StatPart` to modify stats related to the freshness of corpses.

### XML Integration

- **Defining Edible Creatures**: XML files define which creatures can consume rotting or desiccated corpses without food poisoning. The integration ensures seamless gameplay adjustment through mod settings.
- **Patching Defs**: XML patches are required to override default behavior and introduce new consumption rules for rotting corpses. Ensure harmony between XML defs and C# code logic.

### Harmony Patching

- Uses Harmony Library to patch and override base game methods safely, ensuring the mod remains stable and compatible with other mods.
- Key methods are patched to adjust ingestible properties and to prevent food poisoning for designated creatures when consuming specific corpse types.

### Suggestions for Copilot

- **Write Descriptive Comments**: Use comments to describe the purpose and functionality of each method, class, and patch to aid AI understanding.
- **Unit Tests**: Encourage Copilot to suggest unit tests for critical patches and settings, ensuring mod stability.
- **Suggest Efficient Algorithms**: Leverage Copilot to suggest optimized methods for searching and modifying food source logic.
- **Code Refactoring**: Utilize Copilot to propose refactoring opportunities for improving code readability and performance.
- **Documentation**: Prompt Copilot to generate documentation for complex functionality and user guides.

By following these guidelines, developers can effectively use GitHub Copilot to enhance and maintain the "Some Like It Rotten" mod, ensuring a consistent and enjoyable experience for RimWorld players.
