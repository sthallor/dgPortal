using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IgnitionSQLiteTagGen
{
    public class Program
    {
        public static string[] rigs = new[] { "140", "144", "147", "153", "156", "548", "549", "550", "760", "770", "771", "775", "778", "785", "T047", "T052", "T121" };
        //public static string[] rigs = new[] { "T047", "T052", "T121" };
        static void Main(string[] args)
        {
            List<SimpleTagExpressionGenerator> tags = new List<SimpleTagExpressionGenerator>();

            NestedTagExpressionGenerator nteg = new NestedTagExpressionGenerator("Hole Depth", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/hole_depth_ft", 0.3048));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/hole_depth_m", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/hole_depth_m", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_iomap/hole_depth_m", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Bit Depth", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/bit_depth_ft", 0.3048));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/bit_depth_m", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/bit_depth_m", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_iomap/bit_depth_m", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Hookload", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/hook_load_klbs", 444.82));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/hook_load_dan", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/hook_load_dan", 1));
            nteg.Add(new TagPathExpression("s1500/dw_blockinfo/hookloadfiltered_dan", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_iomap/hookloadunfilteredpv_dan", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Weight On Bit", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/wt_on_bit_klbs", 444.82));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/wt_on_bit_dan", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/wt_on_bit_dan", 1));
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_iomap/weightonbitpv_dan", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_iomap/weightonbitpv_dan", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Block Height", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/block_ht_ft", 0.3048));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/block_ht_m", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/block_ht_m", 1));
            nteg.Add(new TagPathExpression("s1500/dw_blockinfo/quilltippos_m", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_iomap/quilltippospv_m", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Differential Pressure", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/diff_press_psi", 6.8948));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/diff_press_kpa", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/diff_press_kpa", 1));
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_iomap/dp_pressurepv_kpa", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_iomap/dp_pressurepv_kpa", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Top Drive Torque", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/torque_klbs", 1.3558179));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/torque_nm", 0.001));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/torque_nm", 0.001));
            nteg.Add(new TagPathExpression("s1500/td_main/iomap/ai_torque_actual_nm", 0.001));
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_iomap/td_quill_torquepv_nm", 0.001));
            nteg.Add(new TagPathExpression("ensign_ac_rig/control/td/ai/tdtorq", 0.001));
            nteg.Add(new TagPathExpression("adr_pilot/td_main/iomap/ai_torque_actual_knm", 1));
            nteg.Add(new TagPathExpression("adr_pilot/td_main/iomap/ai_torque_actual_nm", 0.001));
            nteg.Add(new TagPathExpression("adr_pilot/td_main/iomap/ai_torque_actual_knm", 1));
            nteg.Add(new TagPathExpression("adr_pilot/td_main/iomap/ai_torque_actual_nm", 0.001));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Rate of Penetration", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/rop_fph", 0.3048));
            nteg.Add(new TagPathExpression("Pason/rop_mps", 3600));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/rop_mps", 3600));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/rop_mps", 3600));
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_iomap/rop_mps", 3600));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_iomap/rop_mps", 3600));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Standpipe Pressure", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/pressure_psi", 6.8948));
            nteg.Add(new TagPathExpression("s1500/mp_groupmgr/iomap/ai_standpipepress_kpa", 1));
            nteg.Add(new TagPathExpression("s1500/mp_groupmgr/ictrl/standpipepressurepv_kpa", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_iomap/standpipepressunfiltpv_kpa", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Pump 1 SPM", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/strokes_1_spm", 1));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/strokes_1_spm", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/strokes_1_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp01mgr/ictrl/speedpv_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp_groupmgr/fbdbi/mp01_speed_pot_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp01mgr/ictrl/speedcv_spm", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/abb_mp1/speed", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Pump 2 SPM", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/strokes_2_spm", 1));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/strokes_2_spm", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/strokes_2_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp02mgr/ictrl/speedpv_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp_groupmgr/fbdbi/mp02_speed_pot_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp02mgr/ictrl/speedcv_spm", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/abb_mp2/speed", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Pump 3 SPM", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("Pason/strokes_3_spm", 1));
            nteg.Add(new TagPathExpression("s1500/pason/pason_edr_iomap/strokes_3_spm", 1));
            nteg.Add(new TagPathExpression("adr_pilot/pason/pason_edr_iomap/strokes_3_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp03mgr/ictrl/speedpv_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp_groupmgr/fbdbi/mp03_speed_pot_spm", 1));
            nteg.Add(new TagPathExpression("s1500/mp03mgr/ictrl/speedcv_spm", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/abb_mp3/speed", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Generator 1 KW", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("s1500/power/pwr_genset01_status/total_active_power_kw", 1));
            nteg.Add(new TagPathExpression("s1500/pwr_genset_01_gen/pwr_genset_iomap_woodward/tot_real_pow_kw", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/generator1/total_kw", 1));
            nteg.Add(new TagPathExpression("cameron/hitech/plc/generator/1/power/fdbkkw", 1));
            nteg.Add(new TagPathExpression("trinidad omron opc/drivehouse plc/power/gen1/status/gen1power_kw", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/wward1/kw", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Generator 2 KW", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("s1500/power/pwr_genset02_status/total_active_power_kw", 1));
            nteg.Add(new TagPathExpression("s1500/pwr_genset_02_gen/pwr_genset_iomap_woodward/tot_real_pow_kw", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/generator2/total_kw", 1));
            nteg.Add(new TagPathExpression("cameron/hitech/plc/generator/2/power/fdbkkw", 1));
            nteg.Add(new TagPathExpression("trinidad omron opc/drivehouse plc/power/gen2/status/gen2power_kw", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/wward2/kw", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Generator 3 KW", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("s1500/power/pwr_genset03_status/total_active_power_kw", 1));
            nteg.Add(new TagPathExpression("s1500/pwr_genset_03_gen/pwr_genset_iomap_woodward/tot_real_pow_kw", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/generator3/total_kw", 1));
            nteg.Add(new TagPathExpression("cameron/hitech/plc/generator/3/power/fdbkkw", 1));
            nteg.Add(new TagPathExpression("trinidad omron opc/drivehouse plc/power/gen3/status/gen3power_kw", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/wward3/kw", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Generator 4 KW", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("s1500/power/pwr_genset04_status/total_active_power_kw", 1));
            nteg.Add(new TagPathExpression("s1500/pwr_genset_04_gen/pwr_genset_iomap_woodward/tot_real_pow_kw", 1));
            nteg.Add(new TagPathExpression("ensign_ac_rig/generator4/total_kw", 1));
            nteg.Add(new TagPathExpression("trinidad omron opc/drivehouse plc/power/gen4/status/gen4power_kw", 1));
            nteg.Add(new TagPathExpression("cameron/hitech/plc/generator/4/power/fdbkkw", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("WOB Set Point", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_recipe/p_wob_user_sp_dan", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_recipe/p_wob_user_sp_dan", 1));
            nteg.Add(new TagPathExpression("s1500/autodriller/p_autodrillersetpoints_usedinmanager/Wobusersetpoint_Dan", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/p_autodrillersetpoints_usedinmanager/Wobusersetpoint_Dan", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Differential Pressure Set Point", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_recipe/p_dp_user_sp_kpa", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_recipe/p_dp_user_sp_kpa", 1));
            nteg.Add(new TagPathExpression("s1500/autodriller/p_autodrillersetpoints_usedinmanager/Dpusersetpoint_Kpa", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/p_autodrillersetpoints_usedinmanager/Dpusersetpoint_Kpa", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Top Drive Torque Set Point", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_recipe/p_tdt_user_sp_nm", 0.001));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_recipe/p_tdt_user_sp_nm", 0.001));
            nteg.Add(new TagPathExpression("s1500/autodriller/p_autodrillersetpoints_usedinmanager/Torqueusersetpoint_Nm", 0.001));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/p_autodrillersetpoints_usedinmanager/Torqueusersetpoint_Nm", 0.001));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("ROP Set Point", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_recipe/p_rop_user_sp_mps", 3600));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_recipe/p_rop_user_sp_mps", 3600));
            nteg.Add(new TagPathExpression("s1500/autodriller/p_autodrillersetpoints_usedinmanager/Ropusersetpoint_Mps", 3600));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/p_autodrillersetpoints_usedinmanager/Ropusersetpoint_Mps", 3600));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("QO Left Torque", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("adr_pilot/td_main/params/p_drill_rev_torque_preset_knm", 1));
            nteg.Add(new TagPathExpression("s1500/topdrive/tdn_mode_osc_base_params_db/p_rev_quill_maxscalartorquesp_nm", 0.001));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("QO Right Torque", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("adr_pilot/td_main/params/p_drill_fwd_torque_preset_knm", 1));
            nteg.Add(new TagPathExpression("s1500/topdrive/tdn_mode_osc_base_params_db/p_fwd_quill_maxscalartorquesp_nm", 0.001));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("QO Left Degrees", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("adr_pilot/td_main/params/p_drill_rev_degrees", 1));
            nteg.Add(new TagPathExpression("s1500/topdrive/tdn_mode_osc_base_params_db/p_rev_quill_endpoint_deg", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("QO Right Degrees", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("adr_pilot/td_main/params/p_drill_fwd_degrees", 1));
            nteg.Add(new TagPathExpression("s1500/topdrive/tdn_mode_osc_base_params_db/p_fwd_quill_endpoint_deg", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("QO Left Revolutions", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("adr_pilot/td_main/params/p_drill_rev_revolutions", 1));
            nteg.Add(new TagPathExpression("s1500/topdrive/tdn_mode_osc_base_params_db/p_rev_quill_endpoint_revolutions", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("QO Right Revolutions", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("adr_pilot/td_main/params/p_drill_fwd_revolutions", 1));
            nteg.Add(new TagPathExpression("s1500/topdrive/tdn_mode_osc_base_params_db/p_fwd_quill_endpoint_revolutions", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("QO Left RPM", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("adr_pilot/td_main/params/p_drill_rev_speed_preset_rpm", 1));
            nteg.Add(new TagPathExpression("s1500/topdrive/tdn_mode_osc_base_params_db/p_rev_quill_speedsp_rpm", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("QO Right RPM", SimpleTagExpressionGenerator.TagTypeEnum.Float);
            nteg.Add(new TagPathExpression("adr_pilot/td_main/params/p_drill_fwd_speed_preset_rpm", 1));
            nteg.Add(new TagPathExpression("s1500/topdrive/tdn_mode_osc_base_params_db/p_fwd_quill_speedsp_rpm", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Top Drive Torque Set Point Active", SimpleTagExpressionGenerator.TagTypeEnum.Boolean);
            nteg.Add(new TagPathExpression("s1500/autodriller/p_AutoDrillerSetPoints_UsedInManager/TorqueAxisEnable", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/p_AutoDrillerSetPoints_UsedInManager/TorqueAxisEnable", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_recipe/p_tdt_enablecmd", 1));
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_recipe/p_tdt_enablecmd", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Differential Pressure Set Point Active", SimpleTagExpressionGenerator.TagTypeEnum.Boolean);
            nteg.Add(new TagPathExpression("s1500/autodriller/p_AutoDrillerSetPoints_UsedInManager/DPAxisEnable", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/p_AutoDrillerSetPoints_UsedInManager/DPAxisEnable", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_recipe/p_dp_enablecmd", 1));
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_recipe/p_dp_enablecmd", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("Weight On Bit Set Point Active", SimpleTagExpressionGenerator.TagTypeEnum.Boolean);
            nteg.Add(new TagPathExpression("s1500/autodriller/p_AutoDrillerSetPoints_UsedInManager/WOBAxisEnable", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/p_AutoDrillerSetPoints_UsedInManager/WOBAxisEnable", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_recipe/p_wob_enablecmd", 1));
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_recipe/p_wob_enablecmd", 1));
            tags.Add(nteg);
            nteg = new NestedTagExpressionGenerator("ROP Set Point Active", SimpleTagExpressionGenerator.TagTypeEnum.Boolean);
            nteg.Add(new TagPathExpression("s1500/autodriller/p_AutoDrillerSetPoints_UsedInManager/ROPAxisEnable", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/p_AutoDrillerSetPoints_UsedInManager/ROPAxisEnable", 1));
            nteg.Add(new TagPathExpression("adr_pilot/autodriller/ad_recipe/p_rop_enablecmd", 1));
            nteg.Add(new TagPathExpression("s1500/autodriller/ad_recipe/p_rop_enablecmd", 1));
            tags.Add(nteg);

            SimpleTagExpressionGenerator steg = new SimpleTagExpressionGenerator("Rig State", SimpleTagExpressionGenerator.TagTypeEnum.String);
            steg.TagExpression = "if (isnull({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]),null,if((dateDiff(fromMillis(toLong({[~]edge_analytics/rig_state_dataset}[0,\"t_stamp\"])), now(), \"minute\") > 10),\"Error In Data\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=0,\"Data Incomplete\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=2,\"Error In Data\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=5,\"Other\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=20,\"Tripping In\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=25,\"Trip In Connect\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=30,\"Tripping Out\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=35,\"Trip Out Connect\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=40,\"Back Reaming\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=50,\"Reaming\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=60,\"Circulating\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=70,\"Connecting\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=80,\"Slide Drilling\",if({[~]edge_analytics/rig_state_dataset}[0,\"floatvalue\"]=90,\"Rotate Drilling\",\"unknown\")))))))))))))))";
            tags.Add(steg);

            //using (StreamWriter w = new StreamWriter(@"C:\temp\SQLLiteTagGen\xxx.csv"))
            //{
            //    foreach (string rig in rigs)
            //    {
            //        foreach (NestedTagExpressionGenerator tagGen in tags.Where(tg => tg is NestedTagExpressionGenerator).Cast<NestedTagExpressionGenerator>().OrderBy(tg => tg.TagName))
            //        {
            //            foreach (TagPathExpression tpe in tagGen.TagPathExpressions.OrderBy(tpe => tpe.TagPath))
            //            {
            //                w.WriteLine("{0},{1}", rig, tpe.TagPath);
            //            }
            //        }
            //        w.WriteLine("{0},edge_analytics/rig_state_dataset", rig);
            //    }
            //}
            foreach (string rig in rigs)
            {
                using (StreamWriter w = new StreamWriter(string.Format(@"C:\temp\SQLLiteTagGen\SQLiteTagGen_rig{0}.sql", rig)))
                {
                    w.WriteLine(StringTemplates.SqlLiteScanClassCreateSql);
                    w.WriteLine(string.Format(StringTemplates.SqlLiteTagFolderCreateSql, rig));
                    foreach (SimpleTagExpressionGenerator tagGen in tags)
                    {
                        w.WriteLine(string.Format(StringTemplates.SqlLiteTagGenSql, tagGen.TagName, rig, tagGen.GetTagExpression(), tagGen.TagTypeString));
                    }
                }
            }
        }
    }
}
