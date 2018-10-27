////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin
// @author Koretsky Konstantin (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text;
using SA.Foundation.Templates;
using SA.iOS.GameKit;
using SA.iOS.Foundation;
using UnityEngine;

namespace SA.iOS.Examples
{

    public class GameKitUseExample : MonoBehaviour
    {

        private List<ISN_GKSavedGame> m_fetchedSavedGames = new List<ISN_GKSavedGame>();
        private List<string> m_conflictedSavedGames = new List<string>();

        void Awake() {

            ISN_GKLocalPlayerListener.DidModifySavedGame.AddListener(DidModifySavedGame);
            ISN_GKLocalPlayerListener.HasConflictingSavedGames.AddListener(HasConflictingSavedGames);


            ISN_GKAchievement.LoadAchievements((result) => {
                if (result.IsSucceeded) {
                    foreach (ISN_GKAchievement achievement in result.Achievements) {
                        Debug.Log(achievement.Identifier);
                        Debug.Log(achievement.PercentComplete);
                        Debug.Log(achievement.LastReportedDate);
                        Debug.Log(achievement.Completed);
                    }
                } else {
                    Debug.Log("LoadAchievements failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
                }
            });




            ISN_GKAchievement.ResetAchievements((result) => {
                if (result.IsSucceeded) {
                    Debug.Log("Reset Achievements Success");
                } else {
                    Debug.Log("Reset Achievements failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
                }
            });

            ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
            viewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;
            viewController.Show();


            ISN_GKScore scoreReporter = new ISN_GKScore("itunes.leaderboard.id");
            scoreReporter.Value = 100;
            scoreReporter.Context = 1;


            scoreReporter.Report((result) => {
                if (result.IsSucceeded) {
                    Debug.Log("Score Report Success");
                } else {
                    Debug.Log("Score Report failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
                }
            });


            ISN_GKScore scoreReporter1 = new ISN_GKScore("itunes.leaderboard.id.1");
            scoreReporter1.Value = 100;
            scoreReporter1.Context = 1;


            ISN_GKScore scoreReporter2 = new ISN_GKScore("itunes.leaderboard.id.2");
            scoreReporter2.Value = 100;
            scoreReporter2.Context = 1;

            var scores = new List<ISN_GKScore>() {scoreReporter1, scoreReporter2};

            ISN_GKScore.ReportScores(scores, (result) => {
                if (result.IsSucceeded) {
                    Debug.Log("Score Report Success");
                } else {
                    Debug.Log("Score Report failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
                }
            });

            var leaderboardRequest = new ISN_GKLeaderboard();
            leaderboardRequest.Identifier = "itunes.leaderboard.id";
            leaderboardRequest.PlayerScope = ISN_GKLeaderboardPlayerScope.Global;
            leaderboardRequest.TimeScope = ISN_GKLeaderboardTimeScope.AllTime;
            leaderboardRequest.Range = new ISN_NSRange(1, 10);
            leaderboardRequest.LoadScores((result) => {
                if (result.IsSucceeded) {
                    Debug.Log("Score Load Success");
                    foreach(var score in result.Scores) {
                        Debug.Log(score.Value);
                        Debug.Log(score.Context);
                        Debug.Log(score.Date);
                        Debug.Log(score.Rank);
                        Debug.Log(score.Player.PlayerID);
                        Debug.Log(score.Player.DisplayName);
                        Debug.Log(score.Player.Alias);
                    }

                    Debug.Log(leaderboardRequest.MaxRange);
                    Debug.Log(leaderboardRequest.LocalPlayerScore.Value);
                    Debug.Log(leaderboardRequest.LocalPlayerScore.Context);
                    Debug.Log(leaderboardRequest.LocalPlayerScore.Date);
                    Debug.Log(leaderboardRequest.LocalPlayerScore.Rank);
                } else {
                    Debug.Log("Score Load failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
                }
            });


            ISN_GKLeaderboard.LoadLeaderboards((result) => {
                if (result.IsSucceeded) {
                    foreach(var leaderboards in result.Leaderboards) {
                        Debug.Log(leaderboards.Identifier);
                        Debug.Log(leaderboards.GroupIdentifier);
                        Debug.Log(leaderboards.Title);
                    }
                } else {
                    Debug.Log("Load Leaderboards failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
                }
            });

            /*

            ISN_GKAchievement achievement = new ISN_GKAchievement("itunes.achievement.id");
            achievement.PercentComplete = 50.0f;
            achievement.Report((result) => {
                if(result.IsSucceeded) {
                    Debug.Log("Achievement reported");
                } else {
                    Debug.Log("Achievement report failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
                }
            });


            ISN_GKAchievement achievement1 = new ISN_GKAchievement("itunes.achievement.id.1");
            achievement.PercentComplete = 50.0f;

            ISN_GKAchievement achievement2 = new ISN_GKAchievement("itunes.achievement.id.2");
            achievement.PercentComplete = 70.0f;

            var achievements = new List<ISN_GKAchievement>() { achievement1, achievement2 };

            ISN_GKAchievement.ReportAchievements(achievements, (result) => {
                if (result.IsSucceeded) {
                    Debug.Log("Achievements reported");
                } else {
                    Debug.Log("Achievements report failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
                }
            });
*/

        }

        void OnDestroy() {
            ISN_GKLocalPlayerListener.DidModifySavedGame.RemoveListener(DidModifySavedGame);
            ISN_GKLocalPlayerListener.HasConflictingSavedGames.RemoveListener(HasConflictingSavedGames);
        }

        /// <summary>
        /// Indicates that saved game data was modified.
        /// This method is usually called when a game is saved on device other than the device currently in use.
        /// </summary>
        private void DidModifySavedGame(ISN_GKSavedGameSaveResult result) {
            Debug.Log("DidModifySavedGame! Device name = " + result.SavedGame.DeviceName + " | game name = " + result.SavedGame.Name + " | modification Date = " + result.SavedGame.ModificationDate.ToString());
        }

        /// <summary>
        /// Invoked when a conflict arises between different versions of the same saved game.
        /// Saved game files conflict when multiple devices write to the same saved game file while one or more of the devices are offline.
        /// The app must determine which saved game data is the correct data to use and then call the ResolveConflicts <see cref="ISN_GKLocalPlayer"/>
        /// </summary>
        private void HasConflictingSavedGames(ISN_GKSavedGameFetchResult result) {
            foreach(ISN_GKSavedGame game in result.SavedGames) {
                m_conflictedSavedGames.Add(game.Id);
            }

            foreach (ISN_GKSavedGame game in result.SavedGames) {
                Debug.Log("HasConflictingSavedGames! Device name = " 
                          + game.DeviceName + " | game name = " 
                          + game.Name + " | modification Date = " 
                          + game.ModificationDate.ToString());
            }
        }

        private void OnGUI() {

            //ISN_GKLocalPlayer info

            if (GUI.Button(new Rect(0, 0, 250, 50), "Authenticate")) {
                ISN_GKLocalPlayer.Authenticate((SA_Result result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("Authenticate is succeeded!");

                        ISN_GKLocalPlayer player = ISN_GKLocalPlayer.LocalPlayer;
                        Debug.Log(player.PlayerID);
                        Debug.Log(player.Alias);
                        Debug.Log(player.DisplayName);
                        Debug.Log(player.Authenticated);
                        Debug.Log(player.Underage);


                        player.GenerateIdentityVerificationSignatureWithCompletionHandler((signatureResult) => {
                            if(signatureResult.IsSucceeded) {
                                Debug.Log("signatureResult.PublicKeyUrl: " + signatureResult.PublicKeyUrl);
                                Debug.Log("signatureResult.Timestamp: " + signatureResult.Timestamp);
                                Debug.Log("signatureResult.Salt.Length: " + signatureResult.Salt.Length);
                                Debug.Log("signatureResult.Signature.Length: " + signatureResult.Signature.Length);
                            } else {
                                Debug.Log("IdentityVerificationSignature has failed: " + signatureResult.Error.FullMessage);
                            }
                        });

                    }
                    else {
                        Debug.Log("Authenticate is failed! Error with code: " + result.Error.Code + " and description: " + result.Error.Message);
                    }
                });
            }



            if (GUI.Button(new Rect(250, 0, 250, 50), "Get GKLocalPlayer")) {
                ISN_GKLocalPlayer localPlayer = ISN_GKLocalPlayer.LocalPlayer;

                Debug.Log("PlayerID: " + localPlayer.PlayerID + " | Alias: " + localPlayer.Alias + " | DisplayName: " + localPlayer.DisplayName);
            }


            if (GUI.Button(new Rect(0, 50, 250, 50), "Is Authenticated?")) {
                Debug.Log(ISN_GKLocalPlayer.LocalPlayer.Authenticated);
            }


            if (GUI.Button(new Rect(250, 50, 250, 50), "Is Underage?")) {
                Debug.Log(ISN_GKLocalPlayer.LocalPlayer.Underage);
            }


            if (GUI.Button(new Rect(0, 200, 250, 50), "Fetch saved games")) {
                ISN_GKLocalPlayer.FetchSavedGames((ISN_GKSavedGameFetchResult result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("Loaded " + result.SavedGames.Count + " saved games");
                        foreach (ISN_GKSavedGame game in result.SavedGames) {
                            Debug.Log(game.Name);
                            Debug.Log(game.DeviceName);
                            Debug.Log(game.ModificationDate);
                        }
                    }
                    else {
                        Debug.Log("Fetching saved games is failed! " +
                        "With: " + result.Error.Code + " and description: " + result.Error.Message);
                    }
                });
            }




            if (GUI.Button(new Rect(250, 200, 250, 50), "Save a game")) {

                byte[] data = Encoding.UTF8.GetBytes("data A");
                Debug.Log("Sends byte array length " + data.Length);

                ISN_GKLocalPlayer.SavedGame("file_name", data, (ISN_GKSavedGameSaveResult result) => {
                    if (result.IsSucceeded) {
                        Debug.Log(result.SavedGame.Name);
                        Debug.Log(result.SavedGame.DeviceName);
                        Debug.Log(result.SavedGame.ModificationDate);
                    } else {
                        Debug.Log("SavedGame is failed! With: " + result.Error.Code + " and description: " + result.Error.Message);
                    }
                });
            }




            if (GUI.Button(new Rect(0, 250, 250, 50), "Save a game 2")) {
                byte[] data = Encoding.UTF8.GetBytes("data AAA");
                Debug.Log("Sends byte array length " + data.Length);

                ISN_GKLocalPlayer.SavedGame("new_file_name", data, (ISN_GKSavedGameSaveResult result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("SavedGame is succeeded! Device name = " + result.SavedGame.DeviceName + " | game name = " + result.SavedGame.Name + " | modification Date = " + result.SavedGame.ModificationDate.ToString());
                    }
                    else {
                        Debug.Log("SavedGame is failed! With: " + result.Error.Code + " and description: " + result.Error.Message);
                    }
                });
            }




            if (GUI.Button(new Rect(250, 250, 250, 50), "Delete a game")) {
                ISN_GKLocalPlayer.DeleteSavedGame(m_fetchedSavedGames[0], (SA_Result result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("DeleteSavedGame is succeeded!");
                    }
                    else {
                        Debug.Log("DeleteSavedGame is failed! Error with code: " + result.Error.Code + " and description: " + result.Error.Message);
                    }
                });
            }


            if (GUI.Button(new Rect(0, 300, 250, 50), "Load saved game data")) {
                ISN_GKLocalPlayer.LoadGameData(m_fetchedSavedGames[0], (ISN_GKSavedGameLoadResult result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("Loading game data is succeeded! " +
                                  "StringData = " + result.StringData + " " +
                                  "byte array length " + result.BytesArrayData.Length);
                    } else {
                        Debug.Log("Loading game data is failed! Error with code: " + result.Error.Code + " and description: " + result.Error.Message);
                    }
                });
            }

            if (GUI.Button(new Rect(250, 300, 250, 50), "Resolve Saved Games Conflicts")) {
                //Choose correct data
                byte[] data = Encoding.UTF8.GetBytes("data AAA");
                Debug.Log("Sends byte array length " + data.Length);
                ISN_GKLocalPlayer.ResolveConflictingSavedGames(m_conflictedSavedGames, data, (ISN_GKSavedGameFetchResult result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("Resolve Conflicted Saved Games is succeeded!");
                    }
                    else {
                        Debug.Log("Resolve Conflicted Saved Games is failed!");
                    }
                });
            }

            /*
            if (GUI.Button(new Rect(0, 500, 250, 50), "Show")) {
                ISN_GKGameCenterViewController gKGameCenterViewController = new ISN_GKGameCenterViewController();
                gKGameCenterViewController.LeaderboardIdentifier = "my_first_leaderboard";
                gKGameCenterViewController.LeaderboardTimeScope = ISN_GKLeaderboardTimeScope.AllTime;
                gKGameCenterViewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;

                ISN_GKGameCenterViewController.Show(JsonUtility.ToJson(gKGameCenterViewController));
            }

*/


            if (GUI.Button(new Rect(0, 400, 250, 50), "Load Achivemnets")) {
                ISN_GKAchievement.LoadAchievements((result) => {
                    if(result.IsSucceeded) {
                        Debug.Log("Loaded: " + result.Achievements.Count + " Achievements");
                       // m_achivemnets = result.Achievements;
                    } else {
                        Debug.Log("LoadAchievements failed! With: " + result.Error.Code + " and description: " + result.Error.Message);
                    }
                });
            }

            if (GUI.Button(new Rect(250, 400, 250, 50), "Reset Achivemnets")) {
                ISN_GKAchievement.ResetAchievements((result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("ResetAchievements succeeded");
                    } else {
                        Debug.Log("LoadAchievements failed! With: " + result.Error.Code + " and description: " + result.Error.Message);
                    }
                });
            }


            if (GUI.Button(new Rect(0, 450, 250, 50), "Report Achievements")) {

                ISN_GKAchievement achievement1 = new ISN_GKAchievement("my_first_achievement");
                achievement1.PercentComplete = 50.0f;

                achievement1.Report((result) => {
                    if (result.IsSucceeded) {
                        Debug.Log("ReportAchievements succeeded");
                    } else {
                        Debug.Log("LoadAchievements failed! With: " + result.Error.Code + " and description: " + result.Error.Message);
                    }

                });
            }

            if (GUI.Button(new Rect(250, 450, 250, 50), "SHOW Achievements UI")) {

                ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
                viewController.ViewState = ISN_GKGameCenterViewControllerState.Achievements;
                viewController.Show();
            }


            if (GUI.Button(new Rect(0, 500, 250, 50), "SHOW Leaderboards ")) {
                ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
                viewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;
                viewController.Show();

            }


            if (GUI.Button(new Rect(250, 500, 250, 50), "SHOW Challenges ")) {
                ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
                viewController.ViewState = ISN_GKGameCenterViewControllerState.Challenges;
                viewController.Show(() => {
                    Debug.Log("Challenges hideed");
                });

            }
        }

    }



}