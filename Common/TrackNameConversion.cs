using System.Collections.Generic;

namespace GT2.TrackNameConversion
{
    public static class TrackNameConversion
    {
        public static List<string> TrackNames = new List<string> { "2p_autumn", "2p_circuit", "2p_highway", "2p_mini", "2p_mountain", "2p_roma", "2p_roma_night", "2p_roma_short", "2p_seattle", "2p_seatt_s", "2p_short", "2p_shortway", "2p_sprint2", "2p_testline", "2p_test_in2", "autumn", "bill_checker", "cart", "cart2p", "checker", "circle30", "circle80", "circuit", "dart_test2", "grindel", "Gtest", "highway", "laguna", "laguna2", "laguna2p", "lagunareverse", "license_roma", "lic_seattle", "l_20", "l_mid1", "l_mid2", "L_pikes_climb", "L_pikes_down", "l_plam", "l_screw", "L_tahiti_dirt", "maxspeed", "mini", "mountain", "new_parmaS", "new_parmaS_2p", "new_parmaS_rev", "nn_dirt_2", "nn_dirt_2_2p", "nn_dirt_2_rev", "nn_dirt_3", "nn_dirt_3_2p", "no_name_dirt", "no_name_dirt_2p", "no_name_dirt_rev", "parma", "parma_2p", "parma_rev", "pikes", "pikes_2p", "pikes_2p_rev", "pikes_rev", "reverse", "rev_autumn", "rev_circuit", "rev_mini", "rev_mountain", "rev_roma", "rev_roma_night", "rev_roma_short", "rev_short", "rev_speed", "rev_sprint2", "rev_testline", "rev_test_in2", "Rhighway", "roma", "roma_night", "roma_short", "Rshortway", "seattle", "seattle_r", "seatt_s", "seatt_s_r", "short", "shortway", "SMtN_License1", "SMtN_License2", "speed", "speed2p", "speedreverse", "spL1", "spL2", "spL3", "sprint2", "s_speed", "tahiti_d_new", "tahiti_d_new_2p", "tahiti_d_new_rev", "tahiti_t", "tahiti_test7", "tahiti_t_2p", "tahiti_t_rev", "TC_lisence", "testline", "test_20", "test_20a", "test_30a", "test_40", "test_40a", "test_cb", "test_hs1", "test_hs2", "test_in2", "test_j1", "test_ko", "test_ko_brind", "test_l1", "test_l2", "test_s1", "test_s2", "test_ul1", "test_ur1", "test_w1", "test_w2", "test_z", "dirt_test1", "dirt_test2", "dirt_test3", "highway2000", "indi", "monte", "mountain_V", "seattle2000", "tahiti_d", "tahiti_test_short", "tahiti_test6", "test", "test_j", "test_s", "test2" };
        public static Dictionary<uint, string> TrackHashes;

        public static string ToTrackName(this uint trackID)
        {
            if (TrackHashes == null)
            {
                TrackHashes = new Dictionary<uint, string>(TrackNames.Count);
                foreach (string trackName in TrackNames)
                {
                    TrackHashes.Add(trackName.ToTrackID(), trackName);
                }
            }

            return TrackHashes.TryGetValue(trackID, out string foundTrackName) ? foundTrackName : null;
        }

        public static uint ToTrackID(this string trackName)
        {
            uint trackID = 0;
            char[] trackNameChars = trackName.ToCharArray();

            foreach (char trackNameChar in trackNameChars)
            {
                byte asciiValue = (byte)trackNameChar;
                var temp1 = trackID << 6;
                var temp2 = trackID >> 26;
                trackID = temp1 | temp2;
                trackID += asciiValue;
            }

            return trackID;
        }
    }
}