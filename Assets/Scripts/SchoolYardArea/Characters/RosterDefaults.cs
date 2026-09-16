using System.Collections.Generic;

namespace SchoolYardArea.Characters
{
    public static class RosterDefaults
    {
        public static IReadOnlyList<RosterEntry> Entries { get; } = new[]
        {
            new RosterEntry("Helena", CharacterRole.Healer, "Spark burst", "Healing circle", 0),
            new RosterEntry("Vivien", CharacterRole.Trickster, "Quick stars", "Short invisibility", 2),
            new RosterEntry("Owen", CharacterRole.Bruiser, "Heavy toss", "Ground slam", 4),
            new RosterEntry("JP", CharacterRole.Speedster, "Fast taps", "Dash strike", 6),
            new RosterEntry("Steve", CharacterRole.Tank, "Slow heavy hit", "Shield bubble", 8),
            new RosterEntry("Beth", CharacterRole.Controller, "Chalk splash", "Slow zone", 10),
            new RosterEntry("Zoe", CharacterRole.Trapper, "Pop shot", "Sticky trap", 12),
            new RosterEntry("Sophia", CharacterRole.Support, "Ribbon shot", "Team boost", 14),
            new RosterEntry("Zane", CharacterRole.Blaster, "Bounce ball", "Big burst", 16),
            new RosterEntry("Dylan", CharacterRole.Sniper, "Long throw", "Charged shot", 18),
            new RosterEntry("Alex", CharacterRole.Balanced, "Straight shot", "Power combo", 20),
            new RosterEntry("Tomasso", CharacterRole.Wildcard, "Curve shot", "Arena whirlwind", 25)
        };
    }

    public readonly struct RosterEntry
    {
        public RosterEntry(string name, CharacterRole role, string primary, string special, int unlockWins)
        {
            Name = name;
            Role = role;
            Primary = primary;
            Special = special;
            UnlockWins = unlockWins;
        }

        public string Name { get; }
        public CharacterRole Role { get; }
        public string Primary { get; }
        public string Special { get; }
        public int UnlockWins { get; }
    }
}
