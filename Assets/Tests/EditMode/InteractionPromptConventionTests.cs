using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using RIMA;

namespace RIMA.Tests
{
    public class InteractionPromptConventionTests
    {
        private static readonly Regex BracketTokenRegex = new Regex(@"\[[A-Z]+\]", RegexOptions.Compiled);

        private static readonly HashSet<string> ApprovedTokenBearingKeys = new HashSet<string>
        {
            "chamber_select.prompt.attune",
            "chamber_select.prompt.unlock",
            "chamber_select.prompt.enter_rift",
            "combat.prompt.execute",
            "reward.prompt.take",
            "death.btn.retry"
        };

        [Test]
        public void PromptLocTables_TokenBearingKeysAreExactlyTheApprovedSet()
        {
            var tokenBearingKeys = new HashSet<string>(
                TrTable()
                    .Concat(EnTable())
                    .Where(entry => BracketTokenRegex.IsMatch(entry.Value))
                    .Select(entry => entry.Key));

            CollectionAssert.AreEquivalent(ApprovedTokenBearingKeys, tokenBearingKeys);
        }

        [Test]
        public void PromptLocTables_ApprovedPromptKeysHaveExactlyOneTokenInTrAndEn()
        {
            foreach (string key in ApprovedTokenBearingKeys)
            {
                Assert.AreEqual(1, BracketTokenRegex.Matches(TrTable()[key]).Count, $"TR token count for {key}");
                Assert.AreEqual(1, BracketTokenRegex.Matches(EnTable()[key]).Count, $"EN token count for {key}");
            }
        }

        [Test]
        public void PromptLocTables_ApprovedPromptTokensMatchBetweenLanguages()
        {
            foreach (string key in ApprovedTokenBearingKeys)
            {
                string trToken = BracketTokenRegex.Match(TrTable()[key]).Value;
                string enToken = BracketTokenRegex.Match(EnTable()[key]).Value;
                Assert.AreEqual(trToken, enToken, $"Token mismatch for {key}");
            }
        }

        [Test]
        public void HudSetInteractionPrompt_RawActionAddsExactlyOneGToken()
        {
            string prompt = ComposeInteractionPrompt("Enter");

            Assert.AreEqual("[G] Enter", prompt);
            Assert.AreEqual(1, Regex.Matches(prompt, @"\[G\]").Count);
        }

        [Test]
        public void HudSetInteractionPrompt_TokenizedActionDoesNotDoublePrepend()
        {
            Assert.AreEqual("[G] Take Reward", ComposeInteractionPrompt("[G] Take Reward"));
            Assert.AreEqual("   [G] Take Reward", ComposeInteractionPrompt("   [G] Take Reward"));
            Assert.AreEqual("[RMB] Execute", ComposeInteractionPrompt("[RMB] Execute"));
        }

        [Test]
        public void RewardPickupHudRoute_LocalizedTakeRewardEndsWithExactlyOneGToken()
        {
            string previousLanguage = Loc.CurrentLanguage;

            try
            {
                foreach (string language in new[] { "tr", "en" })
                {
                    Loc.SetLanguage(language);
                    string prompt = ComposeInteractionPrompt(Loc.T("reward.prompt.take"));

                    StringAssert.StartsWith("[G]", prompt, $"Reward prompt should keep the baked key token in {language}");
                    Assert.AreEqual(1, Regex.Matches(prompt, @"\[G\]").Count, $"Reward prompt G token count in {language}");
                }
            }
            finally
            {
                Loc.SetLanguage(previousLanguage);
            }
        }

        private static string ComposeInteractionPrompt(string actionName)
        {
            MethodInfo method = typeof(HUDController).GetMethod(
                "ComposeInteractionPrompt",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.IsNotNull(method, "HUDController.ComposeInteractionPrompt must exist");
            return (string)method.Invoke(null, new object[] { actionName });
        }

        private static Dictionary<string, string> TrTable() => LocTable("_tr");

        private static Dictionary<string, string> EnTable() => LocTable("_en");

        private static Dictionary<string, string> LocTable(string fieldName)
        {
            FieldInfo field = typeof(Loc).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(field, $"Loc.{fieldName} must exist");
            return (Dictionary<string, string>)field.GetValue(null);
        }
    }
}
