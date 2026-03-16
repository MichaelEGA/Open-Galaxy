using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class MissionDataFunctions
{
    //This loads or reloads mission data
    public static void LoadMissionData()
    {
        MissionData missionData = GameObject.FindFirstObjectByType<MissionData>();

        if (missionData == null)
        {
            GameObject missionDataGO = new GameObject();
            missionDataGO.name = "missiondata";
            missionData = missionDataGO.AddComponent<MissionData>();
        }

        ClearLists(missionData);
        InitiateLists(missionData);
        LoadInternalCampaignData(missionData);
        LoadExternalCampaignData(missionData);
        OrganiseListsBasedOnDate(missionData);
    }

    //This initiates all the lists used by the  main menu
    public static void InitiateLists(MissionData missionData)
    {
        if (missionData.campaigns == null)
        {
            missionData.campaigns = new List<string>();
        }

        if (missionData.campaignDescriptions == null)
        {
            missionData.campaignDescriptions = new List<string>();
        }

        if (missionData.campaignDates == null)
        {
            missionData.campaignDates = new List<string>();
        }

        if (missionData.mainMissionCampaigns == null)
        {
            missionData.mainMissionCampaigns = new List<string>();
        }

        if (missionData.mainMissionNames == null)
        {
            missionData.mainMissionNames = new List<string>();
        }

        if (missionData.customMissionCampaigns == null)
        {
            missionData.customMissionCampaigns = new List<string>();
        }

        if (missionData.customMissionNames == null)
        {
            missionData.customMissionNames = new List<string>();
        }

    }

    //This clears the lists to prevent duplication when reloading data
    public static void ClearLists(MissionData missionData)
    {
        if (missionData.campaigns != null)
        {
            missionData.campaigns.Clear();
        }

        if (missionData.campaignDescriptions != null)
        {
            missionData.campaignDescriptions.Clear();
        }

        if (missionData.campaignDates != null)
        {
            missionData.campaignDates.Clear();
        }

        if (missionData.mainMissionCampaigns != null)
        {
            missionData.mainMissionCampaigns.Clear();
        }

        if (missionData.mainMissionNames != null)
        {
            missionData.mainMissionNames.Clear();
        }

        if (missionData.customMissionCampaigns != null)
        {
            missionData.customMissionCampaigns.Clear();
        }

        if (missionData.customMissionNames != null)
        {
            missionData.customMissionNames.Clear();
        }
    }

    //This loads all the interal campaign data
    public static void LoadInternalCampaignData(MissionData missionData)
    {
        //This loads the mission data
        UnityEngine.Object[] mainMissions = Resources.LoadAll(OGGetAddress.missions_internal, typeof(TextAsset));

        //This gets all the main campaigns
        if (mainMissions.Length > 0)
        {
            foreach (UnityEngine.Object mission in mainMissions)
            {
                TextAsset missionString = (TextAsset)mission;

                Mission tempMission = JsonUtility.FromJson<Mission>(missionString.text);

                string campaignName = "none";

                bool campaignNodeExists = false;

                foreach (MissionEvent missionEvent in tempMission.missionEventData)
                {
                    if (missionEvent.eventType == "campaigninformation")
                    {
                        bool isPresent = false;

                        campaignName = missionEvent.data1;

                        foreach (string campaign in missionData.campaigns)
                        {
                            if (campaign == missionEvent.data1)
                            {
                                isPresent = true;
                            }
                        }

                        if (isPresent == false)
                        {
                            missionData.campaigns.Add(missionEvent.data1);
                            missionData.campaignDescriptions.Add(missionEvent.data2);
                            missionData.campaignDates.Add(missionEvent.data3);
                        }

                        campaignNodeExists = true;

                        break;
                    }
                }

                missionData.mainMissionNames.Add(mission.name);
                missionData.mainMissionCampaigns.Add(campaignName);

                //This creates a special folder for missions that aren't part of a larger campaign
                if (campaignNodeExists == false)
                {
                    bool campaignExists = false;

                    foreach (string campaign in missionData.campaigns)
                    {
                        if (campaign == "Misc")
                        {
                            campaignExists = true;
                        }
                    }

                    if (campaignExists == false)
                    {
                        missionData.campaigns.Add("Misc");
                        missionData.campaignDescriptions.Add("Single missions of different types.");
                        missionData.campaignDates.Add("");
                    }

                    missionData.customMissionNames.Add(mission.name);
                    missionData.customMissionCampaigns.Add("Misc");
                }
            }
        }
    }

    //This loads all the external campaign data
    public static void LoadExternalCampaignData(MissionData missionData)
    {
        //This gets the external folder
        var info = new DirectoryInfo(OGGetAddress.missions_custom);

        //This loads the mission data
        if (info.Exists == false)
        {
            Directory.CreateDirectory(OGGetAddress.missions_custom);
            info = new DirectoryInfo(OGGetAddress.missions_custom);
        }

        List<TextAsset> customMissionsList = new List<TextAsset>();

        if (info.Exists == true)
        {
            var fileInfo = info.GetFiles("*.json");

            if (fileInfo.Length > 0)
            {
                foreach (FileInfo file in fileInfo)
                {
                    string path = OGGetAddress.missions_custom + file.Name;
                    string missionDataString = File.ReadAllText(path);
                    TextAsset missionDataTextAsset = new TextAsset(missionDataString);
                    missionDataTextAsset.name = System.IO.Path.GetFileNameWithoutExtension(path);
                    customMissionsList.Add(missionDataTextAsset);
                }
            }
        }

        UnityEngine.Object[] customMissions = customMissionsList.ToArray();

        if (customMissions.Length > 0)
        {
            foreach (UnityEngine.Object mission in customMissions)
            {
                TextAsset missionString = (TextAsset)mission;

                Mission tempMission = JsonUtility.FromJson<Mission>(missionString.text);

                string campaignName = "none";

                bool campaignNodeExists = false;

                foreach (MissionEvent missionEvent in tempMission.missionEventData)
                {
                    if (missionEvent.eventType == "campaigninformation")
                    {
                        bool campaignExists = false;

                        campaignName = missionEvent.data1;

                        foreach (string campaign in missionData.campaigns)
                        {
                            if (campaign == missionEvent.data1)
                            {
                                campaignExists = true;
                            }
                        }

                        if (campaignExists == false)
                        {
                            missionData.campaigns.Add(missionEvent.data1);
                            missionData.campaignDescriptions.Add(missionEvent.data2);
                            missionData.campaignDates.Add(missionEvent.data3);
                        }

                        campaignNodeExists = true;

                        break;
                    }
                }

                if (campaignNodeExists == true)
                {
                    missionData.customMissionNames.Add(mission.name);
                    missionData.customMissionCampaigns.Add(campaignName);
                }

                //This creates a special folder for missions that aren't part of a larger campaign
                if (campaignNodeExists == false)
                {
                    bool campaignExists = false;

                    foreach (string campaign in missionData.campaigns)
                    {
                        if (campaign == "Misc")
                        {
                            campaignExists = true;
                        }
                    }

                    if (campaignExists == false)
                    {
                        missionData.campaigns.Add("Misc");
                        missionData.campaignDescriptions.Add("Single missions of different types.");
                        missionData.campaignDates.Add("");
                    }

                    missionData.customMissionNames.Add(mission.name);
                    missionData.customMissionCampaigns.Add("Misc");
                }


            }
        }
        else
        {
            Debug.LogWarning("Open Galaxy found no external files or was unable to load them.");
        }
    }

    //This organises the lists based on the date
    public static void OrganiseListsBasedOnDate(MissionData missionData)
    {
        List<string> campaign = missionData.campaigns;
        List<string> descriptions = missionData.campaignDescriptions;
        List<string> date = missionData.campaignDates;

        // Parse numbers and track valid/invalid
        List<(int index, float? number)> numberData = new List<(int, float?)>();

        for (int i = 0; i < date.Count; i++)
        {
            if (float.TryParse(date[i], out float parsedNumber))
            {
                numberData.Add((i, parsedNumber));
            }
            else
            {
                numberData.Add((i, null));
            }
        }

        // Sort
        List<int> sortedIndices = numberData
            .OrderBy(x => x.number.HasValue ? 0 : 1)
            .ThenBy(x => x.number ?? float.MaxValue)
            .ThenBy(x => x.index)
            .Select(x => x.index)
            .ToList();

        // Reorder all three lists
        missionData.campaigns = sortedIndices.Select(i => campaign[i]).ToList();
        missionData.campaignDescriptions = sortedIndices.Select(i => descriptions[i]).ToList();
        missionData.campaignDates = sortedIndices.Select(i => date[i]).ToList();
    }

    //This loads a custom mission
    public static void LoadExternalMission(string name)
    {
        MissionData missionData = GetMissionData();

        if (missionData != null)
        {
            Task a = new Task(MissionFunctions.RunMission(name, OGGetAddress.missions_custom, true));
        }
    }

    //This loads a main mission
    public static void LoadInternalMission(string name)
    {
        MissionData missionData = GetMissionData();

        if (missionData != null)
        {
            Task a = new Task(MissionFunctions.RunMission(name, OGGetAddress.missions_internal));
        }
    }

    //This gets the mission data object
    public static MissionData GetMissionData()
    {
        MissionData missionData = GameObject.FindAnyObjectByType<MissionData>();

        return (missionData);
    }
}
