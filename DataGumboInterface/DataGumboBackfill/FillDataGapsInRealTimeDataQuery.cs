using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGumboInterface.Service;
using DataGumboInterface.Models.RealTime;

namespace DataGumboBackfill
{
    public class FillDataGapsInRealTimeDataQuery<T> : DataGumboBackfillQuery
    {
        #region static
        public static Dictionary<string, List<TagConfig>> VariableToTagLookup { get; set; }

        static FillDataGapsInRealTimeDataQuery()
        {
            VariableToTagLookup = new Dictionary<string, List<TagConfig>>();
            string varName;
            varName = "Hole Measured Depth"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/hole_depth_ft", 0.3048));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/hole_depth_m", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_iomap/hole_depth_m", 1));
            
            varName = "Bit Measured Depth"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/bit_depth_ft", 0.3048));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/bit_depth_m", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_iomap/bit_depth_m", 1));
            
            varName = "Hookload"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/hook_load_klbs", 444.82));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/hook_load_dan", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/dw_blockinfo/hookloadfiltered_dan", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_iomap/hookloadunfilteredpv_dan", 1));
            
            varName = "Weight On Bit (Calculated)"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/wt_on_bit_klbs", 444.82));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/wt_on_bit_dan", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_iomap/weightonbitpv_dan", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_iomap/weightonbitpv_dan", 1));
            
            varName = "Block Height"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/block_ht_ft", 0.3048));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/block_ht_m", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/dw_blockinfo/quilltippos_m", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_iomap/quilltippospv_m", 1));
            
            varName = "Differential Pressure"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/diff_press_psi", 6.8948));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/diff_press_kpa", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_iomap/dp_pressurepv_kpa", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_iomap/dp_pressurepv_kpa", 1));
            
            varName = "Top Drive Torque"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/torque_klbs", 1.3558179));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/torque_nm ", 0.001));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/td_main/iomap/ai_torque_actual_nm", 0.001));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_iomap/td_quill_torquepv_nm", 0.001));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/control/td/ai/tdtorq", 0.001));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/iomap/ai_torque_actual_knm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/iomap/ai_torque_actual_nm", 0.001));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/iomap/ai_torque_actual_knm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/iomap/ai_torque_actual_nm", 0.001));
            
            varName = "Rate of Penetration (ROP)"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/rop_fph", 0.3048));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/rop_mps", 3600));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_iomap/rop_mps", 3600));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_iomap/rop_mps", 3600));
            
            varName = "Stand Pipe Pressure (SPP)"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/pressure_psi", 6.8948));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp_groupmgr/iomap/ai_standpipepress_kpa", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp_groupmgr/ictrl/standpipepressurepv_kpa", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_iomap/standpipepressunfiltpv_kpa", 1));
            
            varName = "Pump 1 SPM"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/strokes_1_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/strokes_1_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp01mgr/ictrl/speedpv_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp_groupmgr/fbdbi/mp01_speed_pot_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp01mgr/ictrl/speedcv_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/abb_mp1/speed", 1));
            
            varName = "Pump 2 SPM"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/strokes_2_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/strokes_2_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp02mgr/ictrl/speedpv_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp_groupmgr/fbdbi/mp02_speed_pot_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp02mgr/ictrl/speedcv_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/abb_mp2/speed", 1));
            
            varName = "Pump 3 SPM"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("Pason/strokes_3_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pason/pason_edr_iomap/strokes_3_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp03mgr/ictrl/speedpv_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp_groupmgr/fbdbi/mp03_speed_pot_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/mp03mgr/ictrl/speedcv_spm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/abb_mp3/speed", 1));
            
            varName = "Generator 1 KW"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/power/pwr_genset01_status/total_active_power_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pwr_genset_01_gen/pwr_genset_iomap_woodward/tot_real_pow_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/generator1/total_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("cameron/hitech/plc/generator/1/power/fdbkkw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/wward1/kw", 1));
            
            varName = "Generator 2 KW"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/power/pwr_genset02_status/total_active_power_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pwr_genset_02_gen/pwr_genset_iomap_woodward/tot_real_pow_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/generator2/total_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("cameron/hitech/plc/generator/2/power/fdbkkw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/wward2/kw", 1));
            
            varName = "Generator 3 KW"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/power/pwr_genset03_status/total_active_power_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pwr_genset_03_gen/pwr_genset_iomap_woodward/tot_real_pow_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/generator3/total_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("cameron/hitech/plc/generator/3/power/fdbkkw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/wward3/kw", 1));
            
            varName = "Generator 4 KW"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/power/pwr_genset04_status/total_active_power_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/pwr_genset_04_gen/pwr_genset_iomap_woodward/tot_real_pow_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("ensign_ac_rig/generator4/total_kw", 1));
            VariableToTagLookup[varName].Add(new TagConfig("cameron/hitech/plc/generator/4/power/fdbkkw", 1));
            
            varName = "Weight On Bit Set Point"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_recipe/p_wob_user_sp_dan", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_wob_user_sp_dan", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/p_autodrillersetpoints_usedinmanager/Wobusersetpoint_Dan", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/p_autodrillersetpoints_usedinmanager/Wobusersetpoint_Dan", 1));
            
            varName = "Differential Pressure Set Point"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_recipe/p_dp_user_sp_kpa", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_dp_user_sp_kpa", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/p_autodrillersetpoints_usedinmanager/Dpusersetpoint_Kpa", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/p_autodrillersetpoints_usedinmanager/Dpusersetpoint_Kpa", 1));
            
            varName = "Top Drive Torque Set Point"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_recipe/p_tdt_user_sp_nm", 0.001));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_tdt_user_sp_nm", 0.001));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/p_autodrillersetpoints_usedinmanager/Torqueusersetpoint_Nm", 0.001));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/p_autodrillersetpoints_usedinmanager/Torqueusersetpoint_Nm", 0.001));
            
            varName = "ROP Set Point"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_recipe/p_rop_user_sp_mps", 3600));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_rop_user_sp_mps", 3600));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/p_autodrillersetpoints_usedinmanager/Ropusersetpoint_Mps", 3600));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/p_autodrillersetpoints_usedinmanager/Ropusersetpoint_Mps", 3600));
            
            varName = "Left Torque"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/params/p_drill_rev_torque_preset_knm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/topdrive/tdn_mode_osc_base_params_db/p_rev_quill_maxscalartorquesp_nm", 0.001));
            
            varName = "Right Torque"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/params/p_drill_fwd_torque_preset_knm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/topdrive/tdn_mode_osc_base_params_db/p_fwd_quill_maxscalartorquesp_nm", 0.001));
            
            varName = "Left Degrees"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/params/p_drill_rev_degrees", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/topdrive/tdn_mode_osc_base_params_db/p_rev_quill_endpoint_deg", 1));
            
            varName = "Right Degrees"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/params/p_drill_fwd_degrees", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/topdrive/tdn_mode_osc_base_params_db/p_fwd_quill_endpoint_deg", 1));
            
            varName = "Left Revolutions"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/params/p_drill_rev_revolutions", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/topdrive/tdn_mode_osc_base_params_db/p_rev_quill_endpoint_revolutions", 1));
            
            varName = "Right Revolutions"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/params/p_drill_fwd_revolutions", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/topdrive/tdn_mode_osc_base_params_db/p_fwd_quill_endpoint_revolutions", 1));
            
            varName = "Left RPM"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/params/p_drill_rev_speed_preset_rpm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/topdrive/tdn_mode_osc_base_params_db/p_rev_quill_speedsp_rpm", 1));
            
            varName = "Right RPM"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/td_main/params/p_drill_fwd_speed_preset_rpm", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/topdrive/tdn_mode_osc_base_params_db/p_fwd_quill_speedsp_rpm", 1));
            
            varName = "Top Drive Torque Set Point Active"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/p_AutoDrillerSetPoints_UsedInManager/TorqueAxisEnable", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_tdt_enablecmd", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_recipe/p_tdt_enablecmd", 1));
            
            varName = "Differential Pressure Set Point Active"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/p_AutoDrillerSetPoints_UsedInManager/DPAxisEnable", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_dp_enablecmd", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_dp_enablecmd", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_recipe/p_dp_enablecmd", 1));
            
            varName = "Weight On Bit Set Point Active"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/p_AutoDrillerSetPoints_UsedInManager/WOBAxisEnable", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_wob_enablecmd", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_recipe/p_wob_enablecmd", 1));

            varName = "ROP Set Point Active"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/p_AutoDrillerSetPoints_UsedInManager/ROPAxisEnable", 1));
            VariableToTagLookup[varName].Add(new TagConfig("adr_pilot/autodriller/ad_recipe/p_rop_enablecmd", 1));
            VariableToTagLookup[varName].Add(new TagConfig("s1500/autodriller/ad_recipe/p_rop_enablecmd", 1));

            varName = "Rig State"; VariableToTagLookup[varName] = new List<TagConfig>();
            VariableToTagLookup[varName].Add(new TagConfig("edge_analytics/rig_state", 1));
        }
        #endregion

        public FindDataGapsInRealTimeDataQuery<T> FindGapsQuery { get; set; }
        TagConfig BestTagConfig { get; set; }
        public int TagIdForBestTagConfig { get; set; }

        public void Init()
        {
            // given the rig, variable name, and date range, try to figure out the tag and the multiplier

            // determine the ignition rig id
            using (IgnitionEnterpriseReportingEntities context = new IgnitionEnterpriseReportingEntities()) 
            {
                context.Database.CommandTimeout = 300;
                sqlth_drv rig = context.sqlth_drv.FirstOrDefault(sd => sd.nice_name == RigNumber);
                int rigId = rig== null ? 0 : rig.id;

                sqlth_scinfo scinfo = context.sqlth_scinfo.FirstOrDefault(ss => ss.drvid == rigId);
                int scId = scinfo == null ? 0 : scinfo.id;

                IEnumerable<TagConfig> candidateTagConfigs = VariableToTagLookup[VariableName];

                bool bestCandidateHasTagId = false;
                bool bestCandidateHasRecords = false;
                bool bestCandidateHasNonNullRecords = false;

                TagConfig bestCandidate = null;

                foreach (TagConfig candidate in candidateTagConfigs)
                {
                    bool currCandidateHasTagId = false;
                    bool currCandidateHasRecords = false;
                    bool currCandidateHasNonNullRecords = false;

                    sqlth_te te = context.sqlth_te.FirstOrDefault(st => st.scid == scId && st.tagpath == candidate.TagPath && st.retired == null);
                    int tagId = te == null ? 0 : te.id;

                    if (tagId != 0) 
                    {
                        currCandidateHasTagId = true;

                        long queryStTs = RealTimeDataService<T>.ConvertDate(Start);
                        long queryEndTs = RealTimeDataService<T>.ConvertDate(End);

                        int recCount = context.sqlth_1_data.Count(r => r.tagid == tagId && r.t_stamp >= queryStTs && r.t_stamp <= queryEndTs);
                        if (recCount > 0) 
                        {
                            currCandidateHasRecords = true;

                            int nonNullRecCount = context.sqlth_1_data.Count(r => r.tagid == tagId && r.t_stamp >= queryStTs && r.t_stamp <= queryEndTs && (r.intvalue != null || r.floatvalue != null || r.stringvalue != null));

                            if (nonNullRecCount > 0) 
                            {
                                currCandidateHasNonNullRecords = true;
                            }
                        }
                    }

                    if (bestCandidate == null) 
                    {
                        bestCandidate = candidate;
                        bestCandidateHasTagId = currCandidateHasTagId;
                        bestCandidateHasRecords = currCandidateHasRecords;
                        bestCandidateHasNonNullRecords = currCandidateHasNonNullRecords;
                        TagIdForBestTagConfig = tagId;
                    }
                    else
                    {
                        if (!bestCandidateHasTagId && currCandidateHasTagId) {
                            bestCandidate = candidate;
                            bestCandidateHasTagId = currCandidateHasTagId;
                            bestCandidateHasRecords = currCandidateHasRecords;
                            bestCandidateHasNonNullRecords = currCandidateHasNonNullRecords;
                            TagIdForBestTagConfig = tagId;
                        } else if (!bestCandidateHasRecords && currCandidateHasRecords) {
                            bestCandidate = candidate;
                            bestCandidateHasTagId = currCandidateHasTagId;
                            bestCandidateHasRecords = currCandidateHasRecords;
                            bestCandidateHasNonNullRecords = currCandidateHasNonNullRecords;
                            TagIdForBestTagConfig = tagId;
                        } else if (!bestCandidateHasNonNullRecords && currCandidateHasNonNullRecords) {
                            bestCandidate = candidate;
                            bestCandidateHasTagId = currCandidateHasTagId;
                            bestCandidateHasRecords = currCandidateHasRecords;
                            bestCandidateHasNonNullRecords = currCandidateHasNonNullRecords;
                            TagIdForBestTagConfig = tagId;
                        }
                    }

                    if (bestCandidateHasNonNullRecords)
                    {
                        break; // we found a record with non null records - no need to look further
                    }
                }

                if (TagIdForBestTagConfig == 0) {
                    SendDebugMessage("No suitable tags were found for the backfill.  No records will be backfilled.");
                } else {
                    BestTagConfig = bestCandidate;

                    SendDebugMessage(string.Format("If gaps are found, will attempt to fill using tag id {0} ({1}). Multiplier will be {2}.", TagIdForBestTagConfig, BestTagConfig.TagPath, BestTagConfig.Multiplier));
                }
            }
        }

        public override void Execute()
        {
            Init();
            FindGapsQuery = new FindDataGapsInRealTimeDataQuery<T>();
            FindGapsQuery.VariableName = VariableName;
            FindGapsQuery.RigNumber = RigNumber;
            FindGapsQuery.GapThrehold = GapThrehold;
            FindGapsQuery.Start = Start;
            FindGapsQuery.End = End;
            FindGapsQuery.DgbfqeProcessing += HandleEvent;

            FindGapsQuery.Execute();
        }

        public void HandleEvent(DataGumboBackfillQueryEvent evt)
        {
            OnDataGumboBackfillQueryEvent(evt); // this'll handle logging

            DataGumboBackfillQueryDataGapEvent gapEvent = evt as DataGumboBackfillQueryDataGapEvent;
            if (gapEvent != null)
            {
                FillTheGap(gapEvent.GapStart, gapEvent.GapEnd);
            }
        }

        public void FillTheGap(DateTimeOffset startDtm, DateTimeOffset endDtm)
        {
            if (TagIdForBestTagConfig == 0)
            {
                SendErrorMessage("Data gap found but no suitable tag available in ignition. Will not attempt backfill.");
            }
            else
            {
                long startTs = RealTimeDataService<T>.ConvertDate(startDtm);
                long endTs = RealTimeDataService<T>.ConvertDate(endDtm);
                long lastTsWritten = 0L;
                using (IgnitionEnterpriseReportingEntities context = new IgnitionEnterpriseReportingEntities())
                {
                    context.Database.CommandTimeout = 300;
                    IEnumerable<sqlth_1_data_20191016> backfillRecords = context.sqlth_1_data_20191016.Where(sd => sd.tagid == TagIdForBestTagConfig && sd.t_stamp >= startTs && sd.t_stamp <= endTs).OrderBy(sd => sd.t_stamp);
                    foreach (sqlth_1_data_20191016 rec in backfillRecords)
                    {
                        if (rec.t_stamp - lastTsWritten > 9999)
                        {
                            SendWarningMessage(string.Format("Backfill record at {0} ({1}): {2}", RealTimeDataService<T>.ConvertToDate(rec.t_stamp), rec.t_stamp, GetValue(rec)));
                            WriteData(rec.t_stamp, GetValue(rec));
                            lastTsWritten = rec.t_stamp;
                        }
                    }
                    IEnumerable<sqlth_1_data> backfillRecords2 = context.sqlth_1_data.Where(sd => sd.tagid == TagIdForBestTagConfig && sd.t_stamp >= startTs && sd.t_stamp <= endTs).OrderBy(sd => sd.t_stamp);
                    foreach (sqlth_1_data rec in backfillRecords2) 
                    {
                        if (rec.t_stamp - lastTsWritten > 9999)
                        {
                            SendWarningMessage(string.Format("Backfill record at {0} ({1}): {2}", RealTimeDataService<T>.ConvertToDate(rec.t_stamp), rec.t_stamp, GetValue(rec)));
                            WriteData(rec.t_stamp, GetValue(rec));
                            lastTsWritten = rec.t_stamp;
                        }
                    }

                    //RealTimeDataService<T> svc = new RealTimeDataService<T>();
                    //IEnumerable<RealTimeDataTuple<T>> existingRecords = svc.Get(RigNumber, VariableName, startDtm.AddMinutes(-15), endDtm.AddMinutes(15));

                    //foreach (RealTimeDataTuple<T> rec in existingRecords.OrderBy(r => r.Timestamp))
                    //{
                    //    SendDebugMessage(string.Format("Existing record {0}: {1}", rec.Timestamp, rec.Value));
                    //}
                }
            }
        }

        public void WriteData(long ts, T value)
        {
            RealTimeDataService<T> svc = new RealTimeDataService<T>();
            RealTimeDataTuple<T> data = new RealTimeDataTuple<T> { Timestamp = RealTimeDataService<T>.ConvertToDate(ts), Value = value };
            OnDataGumboBackfillQueryEvent(new DataGumboBackfillQueryDataDataWrittenEvent(this, ts, value));
            try
            {
                svc.Put(RigNumber, VariableName, data);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public T GetValue(sqlth_1_data record)
        {
            if (VariableName == "Rig State")
            {
                return (T)Convert.ChangeType(getRigState(record.floatvalue), typeof(T));
            }
            if (typeof(T) == typeof(double))
            {
                return (T)Convert.ChangeType(record.floatvalue * BestTagConfig.Multiplier, typeof(T));
            }
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(record.stringvalue, typeof(T));
            }
            if (typeof(T) == typeof(int))
            {
                return (T)Convert.ChangeType(record.intvalue * BestTagConfig.Multiplier, typeof(T));
            }
            if (typeof(T) == typeof(bool))
            {
                return (T)Convert.ChangeType(record.intvalue, typeof(T));
            }
            return default(T);
        }
        public T GetValue(sqlth_1_data_20191016 record)
        {
            if (VariableName == "Rig State")
            {
                return (T)Convert.ChangeType(getRigState(record.floatvalue), typeof(T));
            }
            if (typeof(T) == typeof(double))
            {
                return (T)Convert.ChangeType(record.floatvalue * BestTagConfig.Multiplier, typeof(T));
            }
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(record.stringvalue, typeof(T));
            }
            if (typeof(T) == typeof(int))
            {
                return (T)Convert.ChangeType(record.intvalue * BestTagConfig.Multiplier, typeof(T));
            }
            if (typeof(T) == typeof(bool))
            {
                return (T)Convert.ChangeType(record.intvalue, typeof(T));
            }
            return default(T);
        }

        public string getRigState(double? value)
        {
            if (value == null)
            {
                return null;
            }
            switch ((int)(value.Value))
            {
                case 0:
                    return "Data Incomplete";
                    break;
                case 2:
                    return "Error In Data";
                    break;
                case 5:
                    return "Other";
                    break;
                case 20:
                    return "Tripping In";
                    break;
                case 25:
                    return "Trip In Connect";
                    break;
                case 30:
                    return "Tripping Out";
                    break;
                case 35:
                    return "Trip Out Connect";
                    break;
                case 40:
                    return "Back Reaming";
                    break;
                case 50:
                    return "Reaming";
                    break;
                case 60:
                    return "Circulating";
                    break;
                case 70:
                    return "Connecting";
                    break;
                case 80:
                    return "Slide Drilling";
                    break;
                case 90:
                    return "Rotate Drilling";
                    break;
                default:
                    return "unknown";
            }
        }
    }

    public class TagConfig
    {
        public TagConfig(string tagPath, double multiplier)
        {
            TagPath = tagPath;
            Multiplier = multiplier;
        }
        public string TagPath { get; set; }
        public double Multiplier { get; set; }
    }
}
