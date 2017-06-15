using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace mySystem
{
    class common
    {
    /********************用户表***********************/
      public static Hashtable user_table=set_user();
      private static Hashtable set_user()
      {
          Hashtable t2=new Hashtable();
          t2.Add("id", "ID");
          t2.Add("name", "姓名");
          t2.Add("password", "密码");
          t2.Add("last_login_time", "上次登录时间");
          t2.Add("role_id", "权限");
          t2.Add("department_id", "部门");
          user_table.Add("user_table", t2);
          return user_table;
      }

      /********************角色表***********************/
      public static Hashtable role_table = set_role();
      private static Hashtable set_role() {
          Hashtable t2 = new Hashtable();
          t2.Add("role_id", "角色ID");
          t2.Add("role_name", "角色名称");
          t2.Add("role_description", "角色的权限");
          role_table.Add("role_table", t2);
          return role_table;       
      }

      /********************部门表***********************/
      public static Hashtable department_table = set_department();
      private static Hashtable set_department()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("department_id", "部门ID");
          t2.Add("departmentname", "部门名称");
          department_table.Add("department_table", t2);
          return department_table;
      }
      /********************订单表***********************/
      public static Hashtable order_table = set_order();
      private static Hashtable set_order()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("order_id", "订单号");
          t2.Add("order_date", "订单日期");
          t2.Add("product_id", "产品序号");
          t2.Add("product_quantity", "产品数量");
          t2.Add("production_instrument", "生产指令编号");
          order_table.Add("order_table", t2);
          return order_table;
      }
        /********************产品表***********************/
      public static Hashtable product_table = set_product();
      private static Hashtable set_product()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("product_id", "产品序号");
          t2.Add("product_name", "产品名称");
          t2.Add("product_pattern", "产品规格");
          t2.Add("main_unit_of_measure", "主计量单位");
          t2.Add("product_price", "产品价格");
          t2.Add("material_component", "原料组成");
          product_table.Add("product_table", t2);
          return product_table;
      }
        /********************原料表***********************/
      public static Hashtable raw_material_table = set_rawmaterial();
      private static Hashtable set_rawmaterial() {
          Hashtable t2 = new Hashtable();
          t2.Add("raw_materila_id","原料代码");
          t2.Add("raw_material_name","原料名称");
          t2.Add("raw_material_code","原料编码");
          t2.Add("raw_material_pattern","原料规格");
          t2.Add("main_unit_of_measure","主计量单位");
          t2.Add("owned_repository","所属仓库");
          raw_material_table.Add("raw_material_table",t2);
          return raw_material_table;
      }
        /********************原料批号表***********************/
      public static Hashtable raw_material_batch_table = set_rawmaterialbatch();
      private static Hashtable set_rawmaterialbatch() {
          Hashtable t2 = new Hashtable();
          t2.Add("batch_id","原料生产批号");
          t2.Add("raw_material_id","原料代码");
          t2.Add("batch","生产批号");
          raw_material_batch_table.Add("raw_material_batch_table",t2);
          return raw_material_batch_table;
      }
        /********************生产指令表***********************/
      public static Hashtable production_instruction_table = set_produtioninstruction();
      private static Hashtable set_produtioninstruction() {
          Hashtable t2 = new Hashtable();
          t2.Add("product_name","产品名称");
          t2.Add("production_instruction", "生产指令");
          t2.Add("production_process", "生产工艺");
          t2.Add("machine_id", "生产设备编号");
          t2.Add("production_start_date", "开始生产日期");
          t2.Add("instruction_description", "生产指令详细信息");
          t2.Add("extrusion_raw_material", "吹膜原料");
          t2.Add("extrusion_raw_material_id", "吹膜物料代码");
          t2.Add("extrusion_raw_material_batch", "吹膜物料批号");
          t2.Add("extrusion_package_specifications", "吹膜包装规格");
          t2.Add("extrusion_receive_the_quantity_of_raw_material", "吹膜领料量");
          t2.Add("chuimo_item", "吹膜卷心管");
          t2.Add("core_tube_parameter", "吹膜卷心管参数");
          t2.Add("core_tube_package_specifications", "吹膜卷心管规格");
          t2.Add("core_tube_receive_the_quantity_of_raw_material", "吹膜卷心管领料量");
          t2.Add("package_item", "包装项目");
          t2.Add("package_raw_material", "包装物料");
          t2.Add("package_specifications", "包装规格");
          t2.Add("package_recevie_the_quantity_of_raw_material", "包装领料量");
          t2.Add("princpal_id", "负责人");
          t2.Add("operator_id", "操作员");
          t2.Add("reviewer_id", "审核员");
          t2.Add("operate_date", "操作日期");
          t2.Add("review_date", "审核日期");
          production_instruction_table.Add("production_instruction_table", t2);
          return production_instruction_table;
      }
        /********************吹膜表***********************/
        public static Hashtable extrusion_table = set_extrusion();
      private static Hashtable set_extrusion() {
          Hashtable t2 = new Hashtable();
          t2.Add("id","吹膜工序序号");
          t2.Add("product_name", "产品名称");
          t2.Add("product_id", "产品代码");
          t2.Add("product_batch", "产品批号");
          t2.Add("prodcution_instruction", "生产指令编号");
          t2.Add("s1_clean_date", "清洁日期");
          t2.Add("s1_flight", "班次");
          t2.Add("s1_reviewer_id", "复核人");
          t2.Add("s1_region_content_result_cleaner_reviewer", "清洁记录");
          t2.Add("s2_item1_qualified", "确认结果1");
          t2.Add("s2_item2_qualified", "确认结果2");
          t2.Add("s2_item3_qualified", "确认结果3");
          t2.Add("s2_item4_qualified", "确认结果4");
          t2.Add("s2_item5_qualified", "确认结果5");
          t2.Add("s2_item6_qualified", "确认结果6");
          t2.Add("s2_item7_qualified", "确认结果7");
          t2.Add("s2_item8_qualified", "确认结果8");
          t2.Add("s2_item9_qualified", "确认结果9");
          t2.Add("s2_item10_qualified", "确认结果10");
          t2.Add("s2_item11_qualified", "确认结果11");
          t2.Add("s2_item12_qualified", "确认结果12");
          t2.Add("s2_item13_qualified", "确认结果13");
          t2.Add("s2_item14_qualified", "确认结果14");
          t2.Add("s2_operator_id", "确认人");
          t2.Add("s2_date", "确认日期");
          t2.Add("s2_reviewer_id", "复核人");
          t2.Add("s2_review_date", "复核日期");
          t2.Add("s3_record_date", "日期");
          t2.Add("s3_record_id", "记录人");
          t2.Add("s3_reviewer_id", "复核人");
          t2.Add("s3_core_specifications", "模芯规格");
          t2.Add("s3_start_preheat_time", "预热开始时间");
          t2.Add("s3_end_preheat_time", "保温结束时间");
          t2.Add("s3_start_insulation_time", "保温开始时间");
          t2.Add("s3_end_insulation_time_1", "保温结束时间");
          t2.Add("s3_end_insulation_time_2", "保温结束时间");
          t2.Add("s4_raw_material_delivery_date", "传料日期");
          t2.Add("s4_time", "时间");
          t2.Add("s4_raw_material_id", "物料代码");
          t2.Add("s4_quantity", "数量");
          t2.Add("s4_kilogram_per_piece", "Kg/件");
          t2.Add("s4_quantity_per_kilogram", "数量/kg");
          t2.Add("s4_is_packed_well", "包装是否完好");
          t2.Add("s4_is_cleaned", "是否清洁合格");
          t2.Add("s4_operator_id", "操作人");
          t2.Add("s4_reviewer_id", "复核人");
          t2.Add("s5_ab1c_raw_material_id", "外中内层（A,B1,C）原料代码");
          t2.Add("s5_b2_raw_material_id", "中层（B2）原料代码");
          t2.Add("s5_ab1c_raw_material_batch", "外中内层（A,B1,C）原料批号");
          t2.Add("s5_b2_raw_material_batch", "中层（B2）原料批号");
          t2.Add("s5_feeding_info", "供料信息");
          t2.Add("s5_ab1c_raw_material_consumption", "外中内层（A,B1,C）原料用量");
          t2.Add("s5_ab1c_raw_material_margin", "外中内层（A,B1,C）原料余量");
          t2.Add("s5_b2_raw_material_consumption", "中层（B2）原料用量");
          t2.Add("s5_b2_raw_material_margin", "中层（B2）原料余量");
          t2.Add("s5_reviewer_id", "复核人");
          t2.Add("s6_production_date","生产日期");
          t2.Add("s6_flight", "班次");
          t2.Add("s6_temperature", "环境温度");
          t2.Add("s6_relative_humidity", "相对湿度");
          t2.Add("s6_reviewer_id", "复核人");
          t2.Add("s6_time", "时间");
          t2.Add("s6_mojuan_number", "膜卷编号");
          t2.Add("s6_mojuan_length", "膜卷长度");
          t2.Add("s6_mojuan_weight", "膜卷重量");
          t2.Add("s6_recorder_id", "记录人");
          t2.Add("s6_outward", "外观");
          t2.Add("s6_width", "宽度");
          t2.Add("s6_max_thickness", "最大厚度");
          t2.Add("s6_min_thickness", "最小厚度");
          t2.Add("s6_aver_thickness", "平均厚度");
          t2.Add("s6_tolerance_thickness", "厚度公差");
          t2.Add("s6_checker_id", "检查人");
          t2.Add("s6_is_qualified", "判定");
          extrusion_table.Add("extrusion_table",t2);
          return extrusion_table;
      }
        /********************吹膜供料系统运行记录表***********************/
      public static Hashtable runnig_record_of_feeding_unit_table = set_runnig_record_of_feeding_unit();
      private static Hashtable set_runnig_record_of_feeding_unit()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("id","记录序号");
          t2.Add("production_instruction", "生产指令");
          t2.Add("flight", "班次");
          t2.Add("check_time", "检查时间");
          t2.Add("is_motor_working", "电机工作是否正常");
          t2.Add("is_ pneumatic_valve_working", "气动阀工作是否正常");
          t2.Add("is_feeding_working", "供料运行是否正常");
          t2.Add("is_alarm", "有无警报显示");
          t2.Add("is_lift_alarm", "是否解除警报");
          t2.Add("checker_id", "检查人");
          t2.Add("reviewer_id", "复核人");
          runnig_record_of_feeding_unit_table.Add("runnig_record_of_feeding_unit_table", t2);
          return runnig_record_of_feeding_unit_table;
      }
        /********************吹膜机组运行记录表***********************/
      public static Hashtable running_record_of_extrusion_unit_table = set_running_record_of_extrusion_unit();
      private static Hashtable set_running_record_of_extrusion_unit()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("id","记录序号");
          t2.Add("product_id", "产品代码");
          t2.Add("product_batch", "产品批号");
          t2.Add("production_date", "生产日期");
          t2.Add("record_time", "记录时间");
          t2.Add("recorder_id", "记录人");
          t2.Add("reviewer_id", "复核人");
          t2.Add("a_1_temperature", "A层一区实际温度");
          t2.Add("a_2_temperature", "A层二区实际温度");
          t2.Add("a_3_temperature", "A层三区实际温度");
          t2.Add("a_4_temperature", "A层四区实际温度");
          t2.Add("a_hw_temperature", "A层换网实际温度");
          t2.Add("a_ld_temperature", "A层流道实际温度");
          t2.Add("b_1_temperature", "B层一区实际温度");
          t2.Add("b_2_temperature", "B层二区实际温度");
          t2.Add("b_3_temperature", "B层三区实际温度");
          t2.Add("b_4_temperature", "B层四区实际温度");
          t2.Add("b_hw_temperature", "B层换网实际温度");
          t2.Add("b_ld_temperature", "B层流道实际温度");
          t2.Add("c_1_temperature", "C层一区实际温度");
          t2.Add("c_2_temperature", "C层二区实际温度");
          t2.Add("c_3_temperature", "C层三区实际温度");
          t2.Add("c_4_temperature", "C层四区实际温度");
          t2.Add("c_hw_temperature", "C层换网实际温度");
          t2.Add("c_ld_temperature", "C层流道实际温度");
          t2.Add("mt_mj_temperature", "模头模颈实际温度");
          t2.Add("mt_1_temperature", "模头一区实际温度");
          t2.Add("mt_2_temperature", "模头二区实际温度");
          t2.Add("mt_km_temperature", "模头口模实际温度");
          t2.Add("mt_line_speed", "模头线速度");
          t2.Add("mt_ld_temperature", "模头流道实际温度");
          t2.Add("item1_set_frequency", "第一牵引设置频率");
          t2.Add("item1_real_frequency", "第一牵引实际频率");
          t2.Add("item1_electric_current", "第一牵引电流");
          t2.Add("item2_set_frequency", "第二牵引设置频率");
          t2.Add("item2_real_frequency", "第二牵引实际频率");
          t2.Add("item2_set_tension", "第二牵引设定张力");
          t2.Add("item2_real_tension", "第二牵引实际张力");
          t2.Add("item2_electric_current", "第二牵引电流");
          t2.Add("item3_set_frequency", "外表面电机设置频率");
          t2.Add("item3_real_frequency", "外表面电机实际频率");
          t2.Add("item3_set_tension", "外表面电机设定张力");
          t2.Add("item3_real_tension", "外表面电机实际张力");
          t2.Add("item3_electric_current", "外表面电机电流");
          t2.Add("item4_set_frequency", "外冷进风机设置频率");
          t2.Add("item4_real_frequency", "外冷进风机实际频率");
          t2.Add("item4_electric_current", "外冷进风机电流");
          t2.Add("a_temperature", "A层下料口温度");
          t2.Add("b_temperature", "B层下料口温度");
          t2.Add("c_temperature", "C层下料口温度");
          t2.Add("push_a_real_frequency", "挤出机A层实际频率");
          t2.Add("push_a_electric_current", "挤出机A层电流");
          t2.Add("push_a_melt_temperature", "挤出机A层熔体温度");
          t2.Add("push_a_pre_melt", "挤出机A层前熔体");
          t2.Add("push_a_after_melt", "挤出机A层后熔压");
          t2.Add("push_a_screw_speed", "挤出机A层螺杆转速");
          t2.Add("push_b_real_frequency", "挤出机B层实际频率");
          t2.Add("push_b_electric_current", "挤出机B层电流");
          t2.Add("push_b_melt_temperature", "挤出机B层熔体温度");
          t2.Add("push_b_pre_melt", "挤出机B层前熔体");
          t2.Add("push_b_after_melt", "挤出机B层后熔压");
          t2.Add("push_b_screw_speed", "挤出机B层螺杆转速");
          t2.Add("push_c_real_frequency", "挤出机C层实际频率");
          t2.Add("push_c_electric_current", "挤出机C层电流");
          t2.Add("push_c_melt_temperature", "挤出机C层熔体温度");
          t2.Add("push_c_pre_melt", "挤出机C层前熔体");
          t2.Add("push_c_after_melt", "挤出机C层后熔压");
          t2.Add("push_c_screw_speed", "挤出机C层螺杆转速");
          running_record_of_extrusion_unit_table.Add("running_record_of_extrusion_unit_table", t2);
          return running_record_of_extrusion_unit_table;
      }
        /********************吹膜工序废品记录表***********************/
      public static Hashtable waste_record_of_extrusion_process_table = set_waste_record_of_extrusion_process();
      private static Hashtable set_waste_record_of_extrusion_process()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("id","记录序号");
          t2.Add("production_instruction", "生产指令");
          t2.Add("start_time_production", "生产开始时间");
          t2.Add("end_time_production", "生产结束时间");
          t2.Add("production_date", "生产日期");
          t2.Add("flight", "班次");
          t2.Add("product_id", "产品代码");
          t2.Add("waste_quantity", "不良品数量");
          t2.Add("cause_of_waste", "废品产生原因");
          t2.Add("recorder_id", "记录人");
          t2.Add("reviewer_id", "复核人");
          waste_record_of_extrusion_process_table.Add("waste_record_of_extrusion_process_table", t2);
          return waste_record_of_extrusion_process_table;
      }
        /********************吹膜工序清场记录表***********************/
      public static Hashtable clean_record_of_extrusion_process_table = set_clean_record_of_extrusion_process();
      private static Hashtable set_clean_record_of_extrusion_process()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("production_instruction", "生产指令");
          t2.Add("product_id_before", "清场前产品代码");
          t2.Add("product_batch_before", "清场前产品批号");
          t2.Add("clean_date", "清场日期");
          t2.Add("item1_is_cleaned", "供料工序1是否清洁");
          t2.Add("item2_is_cleaned", "供料工序2是否清洁");
          t2.Add("item3_is_cleaned", "供料工序3是否清洁");
          t2.Add("item4_is_cleaned", "供料工序4是否清洁");
          t2.Add("item5_is_cleaned", "供料工序5是否清洁");
          t2.Add("item6_is_cleaned", "供料工序6是否清洁");
          t2.Add("item7_is_cleaned", "吹膜工序1是否清洁");
          t2.Add("item8_is_cleaned", "吹膜工序2是否清洁");
          t2.Add("item9_is_cleaned", "吹膜工序3是否清洁");
          t2.Add("item10_is_cleaned", "吹膜工序4是否清洁");
          t2.Add("item11_is_cleaned", "吹膜工序5是否清洁");
          t2.Add("item12_is_cleaned", "吹膜工序6是否清洁");
          t2.Add("item13_is_cleaned", "吹膜工序7是否清洁");
          t2.Add("item14_is_cleaned", "吹膜工序8是否清洁");
 
          t2.Add("cleaner_id", "清场人");
          t2.Add("is_qualified", "检查结果");
          t2.Add("reviewer_id", "检查人");
          clean_record_of_extrusion_process_table.Add("clean_record_of_extrusion_process_table", t2);
          return clean_record_of_extrusion_process_table;
      }
        /********************吹膜工序物料平衡记录表***********************/
      public static Hashtable raw_material_record_of_extrusion_process_table = set_raw_material_record_of_extrusion_process();
      private static Hashtable set_raw_material_record_of_extrusion_process()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("production_instruction", "生产指令");
          t2.Add("production_date", "生产日期");
          t2.Add("finished_product_weight", "成品重量合计");
          t2.Add("waste_weight", "废品量合计");
          t2.Add("middle_feeding", "中层投料量");
          t2.Add("in_out_feeding", "内外层投料量");
          t2.Add("weight_finished_ratio", "重量比成品率");
          t2.Add("product_material_balance", "物料平衡");
          t2.Add("recorder_id", "记录人");
          t2.Add("record_date", "记录日期");
          t2.Add("reviewer_id", "复核人");
          t2.Add("review_date", "复核日期");
          raw_material_record_of_extrusion_process_table.Add("raw_material_record_of_extrusion_process_table", t2);
          return raw_material_record_of_extrusion_process_table;
      }
        /********************吹膜岗位交接班记录表***********************/
      public static Hashtable handover_record_of_extrusion_process_table = set_handover_record_of_extrusion_process();
      private static Hashtable set_handover_record_of_extrusion_process()
      {
          Hashtable t2 = new Hashtable();
          t2.Add("production_date","生产日期");
          t2.Add("production_instruction", "生产指令编号");
          t2.Add("item1_day", "项目1白班确认结果");
          t2.Add("item1_night", "项目1夜班确认结果");
          t2.Add("item2_day", "项目2白班确认结果");
          t2.Add("item2_night", "项目2夜班确认结果");
          t2.Add("item3_day", "项目3白班确认结果");
          t2.Add("item3_night", "项目3夜班确认结果");
          t2.Add("item4_day", "项目4白班确认结果");
          t2.Add("item4_night", "项目4夜班确认结果");
          t2.Add("item5_day", "项目5白班确认结果");
          t2.Add("item5_night", "项目5夜班确认结果");
          t2.Add("item6_day", "项目6白班确认结果");
          t2.Add("item6_night", "项目6夜班确认结果");
          t2.Add("item7_day", "项目7白班确认结果");
          t2.Add("item7_night", "项目7夜班确认结果");
          t2.Add("item8_day", "项目8白班确认结果");
          t2.Add("item8_night", "项目8夜班确认结果");
          t2.Add("item9_day", "项目9白班确认结果");
          t2.Add("item9_night", "项目9夜班确认结果");
          t2.Add("item10_day", "项目10白班确认结果");
          t2.Add("item10_night", "项目10夜班确认结果");
          t2.Add("item11_day", "项目11白班确认结果");
          t2.Add("item11_night", "项目11夜班确认结果");
          t2.Add("item12_day", "项目12白班确认结果");
          t2.Add("item12_night", "项目12夜班确认结果");
          t2.Add("item13_day", "项目13白班确认结果");
          t2.Add("item13_night", "项目13夜班确认结果");
          t2.Add("item14_day", "项目14白班确认结果");
          t2.Add("item14_night", "项目14夜班确认结果");
          t2.Add("product_id_day", "生产产品代码白班");
          t2.Add("product_id_night", "生产产品代码夜班");
          t2.Add("product_batch_day", "产品批号白班");
          t2.Add("product_batch_night", "产品批号夜班");
          t2.Add("product_quantity_day", "产品数量白班");
          t2.Add("product_quantity_night", "产品数量夜班");
          t2.Add("exception_handling_day", "白班异常情况处理");
          t2.Add("to_attend_day", "白班交班人");
          t2.Add("successor_day", "白班接班人");
          t2.Add("successor_time_day", "白班接班时间");
          t2.Add("exception_handling_night", "夜班异常情况处理");
          t2.Add("to_attent_night", "夜班交班人");
          t2.Add("successor_night", "夜班接班人");
          t2.Add("successor_time_night", "夜班接班时间");
          handover_record_of_extrusion_process_table.Add("handover_record_of_extrusion_process_table", t2);
          return handover_record_of_extrusion_process_table;
      }
    }
}
