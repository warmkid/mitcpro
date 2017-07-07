using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mySystem.db
{
    public class Extrusion:DataBase
    {
        //A下拉菜单
        public class T批生产记录封面:Table
        {
            public class Cols:BaseCols
            {
                //  ly
                // 这张表在每一个『生产指令』被审核后自动添加对应的行，把能填的数据都填上
                // 在生产指令结束后，才写入其他数据，其他时间数据都是实时从其他表中读出来。
                // 当 填写完 清场记录时，表示生产指令已完成，此时该表数据需要更新
                // T批生产记录产品详细信息 和 T批生产记录目录详细信息 通过 本表的ID 和本表相关联
                // T批生产记录产品详细信息 和 T批生产记录目录详细信息 也是在生成指令完成后才写入数据
                // T批生产记录目录详细信息 按照默认顺序填入记录名称和序号，页码最后才能填
                public Int32 生产指令ID;
                public String 生产指令编号;
                public String 使用物料;
                public DateTime 开始生产时间;
                public DateTime 结束生产时间;
                public String 汇总人;
                public DateTime 汇总时间;
                public String 审核人;
                public DateTime 审核时间;
                public String 批准人;
                public DateTime 批准时间;
                public String 备注;
                public String 审核意见;
                public Boolean 审核是否通过;
                //  ly
            }
            public T批生产记录封面()
            {
                cols = new Cols();
                tblName = "批生产记录表";
            }
        }

        public class T批生产记录产品详细信息:Table
        {
          public class Cols:BaseCols{
              // 外键
              public Int32 T批生产记录封面ID;
              public String 产品代码;
              public String 产品批号;
              public Double 生产数量;
          }
          public T批生产记录产品详细信息()
          {
              cols = new Cols();
              tblName = "批生产记录产品详细信息";
          }
        }

        public class T批生产记录目录详细信息 : Table
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 T批生产记录封面ID;
                public Int32 序号;
                public String 记录;
                public Int32 页数;
            }

            public T批生产记录目录详细信息()
            {
                tblName = "批生产记录目录详细信息";
                cols = new Cols();
            }
        }

        public class T生产指令信息 : Table
        {
            public class Cols : BaseCols
            {
                public String 产品名称;
                public String 生产指令编号;
                public String 生产工艺;
                public String 生产设备编号;
                public DateTime 开始生产日期;
                public String 内外层物料代码;
                public String 内外层物料批号;
                public String 内外层包装规格;
                public Double 内外层领料量;
                public String 中层物料代码;
                public String 中层物料批号;
                public String 中层包装规格;
                public Double 中层领料量;
                public String 卷心管;
                public String 卷心管规格;
                public Double 卷心管领料量;
                //public String 吹膜工序标签包装规格;
                //public Double 吹膜工序标签领料量;
                public String 双层洁净包装包装规格;
                public Double 双层洁净包装领料量;
                public String 负责人;
                public String 编制人;
                public DateTime 编制时间;
                public String 审批人;
                public DateTime 审批时间;
                public String 接收人;
                public DateTime 接收时间;
                //public Int32 产品列表ID;
                public String 审核意见;
                public Boolean 审核是否通过;
                public Int32 状态;//0（草稿） 1（未接收） 2（已接收） 3（生产中） 4（生产完成）
                public String 备注;
                public Double 计划产量合计米;
                public Double 用料重量合计;
                public Double 计划产量合计卷;
            }

            public T生产指令信息()
            {
                cols = new Cols();
                tblName = "生产指令信息表";
            }
        }

        public class T生产指令产品列表 : Table
        {
            public class Cols : BaseCols
            {
                public Int32 生产指令ID;
                public Int32 序号;
                public String 产品编码;
                public Double 用料重量;
                public String 产品批号;
                public Double 每卷长度;
                public Double 计划产量;
                public Double 卷心管规格;
                public String 产品用途;
                public Int32 标签;
                public Int32 标签领料量;               

            }

            public T生产指令产品列表()
            {
                cols = new Cols();
                tblName = "生产指令产品列表";
            }

        }

        public class T吹膜机组清洁记录 : Table
        {
            public class Cols : BaseCols
            {
                public Int32 生产指令ID;
                //public Int32 清洁项目记录ID;
                public DateTime 清洁日期;
                public Boolean 班次;
                public String 审核人;
                public DateTime 审核时间;
                public String 审核意见;
                public Boolean 审核是否通过;
            }

            public T吹膜机组清洁记录()
            {
                cols = new Cols();
                tblName = "吹膜机组清洁记录表";
            }
        }

        public class T吹膜机组清洁项目记录:Table//通过吹膜机组清洁记录ID连接
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 吹膜机组清洁记录ID;
                public String 清洁区域;
                public String 清洁内容;
                public Boolean 合格_是;
                public Boolean 合格_否;
                public String 清洁人;
                public String 检查人;
            }

            public T吹膜机组清洁项目记录()
            {
                cols = new Cols();
                tblName = "吹膜机组清洁项目记录表";
            }
        }

        public class T吹膜工序清场记录 : Table
        {
            public class Cols : BaseCols
            {
                public Int32 生产指令ID;
                //public Int32 吹膜工序清场项目记录ID;
                public String 生产指令;
                public String 清场前产品代码;
                public String 清场前产品批号;
                public DateTime 清场日期;
                public Boolean 检查结果;
                public String 清场人;
                public String 检查人;
                public String 审核人; //与检查人是否同一个人？
                public DateTime 审核时间;
                public String 审核意见;
                public Boolean 审核是否通过;
            }

            public T吹膜工序清场记录()
            {
                cols = new Cols();
                tblName = "吹膜工序清场记录";
            }
        }

        public class T吹膜工序清场供料工序项目记录 : Table
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 吹膜工序清场记录ID;
                public Int32 序号;
                public String 清场要点;
                public Boolean 清场操作_是;
                public Boolean 清场操作_否;
            }

            public T吹膜工序清场供料工序项目记录()
            {
                cols = new Cols();
                tblName = "吹膜工序清场项目记录";
            }
        }

        public class T吹膜工序清场吹膜工序项目记录 : Table
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 吹膜工序清场记录ID;
                public Int32 序号;
                public String 清场要点;
                public Boolean 清场操作_是;
                public Boolean 清场操作_否;
            }

            public T吹膜工序清场吹膜工序项目记录()
            {
                tblName = "吹膜工序清场吹膜工序项目记录";
                cols = new Cols();
            }
        }

        public class T吹膜岗位交接班记录 : Table
        {
            public class Cols : BaseCols
            {
                public Int32 生产指令ID;
                //public Int32 吹膜岗位交接班确认记录ID;
                public String 生产指令编号;
                public DateTime 生产日期;
                public String 白班产品代码批号数量;
                public String 白班异常情况处理;
                public String 白班交班人;
                public String 白班接班人;
                public DateTime 白班交接班时间;
                public String 夜班产品代码批号数量;
                public String 夜班异常情况处理;
                public String 夜班交班人;
                public String 夜班接班人;
                public DateTime 夜班交接班时间;
            }

            public T吹膜岗位交接班记录()
            {
                tblName = "吹膜岗位交接班记录";
                cols = new Cols();
            }
        }

        public class T吹膜岗位交接班项目记录 : Table
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 T吹膜岗位交接班记录ID;
                public Int32 序号;
                public String 确认项目;
                public Boolean 确认结果白班_是;
                public Boolean 确认结果白班_否;
                public Boolean 确认结果夜班_是;
                public Boolean 确认结果夜班_否;
            }

            public T吹膜岗位交接班项目记录()
            {
                tblName = "吹膜岗位交接班项目记录";
                cols = new Cols();
            }
        }

        //其他表
        public class T设置吹膜产品:Table
        {
            public class Cols : BaseCols
            {
                public String 产品名称;
            }

            public T设置吹膜产品()
            {
                tblName = "设置吹膜产品";
                cols = new Cols();
            }
        }

        public class T设置吹膜产品编码 : Table
        {
            public class Cols : BaseCols
            {
                public String 产品编码;
            }

            public T设置吹膜产品编码()
            {
                tblName = "设置吹膜产品编码";
                cols = new Cols();
            }
        }

        public class T设置物料代码:Table
        {
            public class Cols : BaseCols
            {
                public String 物料代码;
            }

            public T设置物料代码()
            {
                tblName = "设置物料代码";
                cols = new Cols();
            }
        }

        public class T设置吹膜机组清洁项目 : Table
        {
            public class Cols : BaseCols
            {
                public String 清洁区域;
                public String 清洁内容;
            }

            public T设置吹膜机组清洁项目()
            {
                tblName = "设置吹膜机组清洁项目";
                cols = new Cols();
            }
        }

        public class T设置岗位交接班项目 : Table
        {
            public class Cols : BaseCols
            {
                public String 确认项目;
            }

            public T设置岗位交接班项目()
            {
                tblName = "设置岗位交接班项目";
                cols = new Cols();
            }
        }

        public class T设置吹膜机组开机前确认项目 : Table
        {
            public class Cols : BaseCols
            {
                
               // public Int32 序号;
                public String 确认项目;
                public String 确认内容;
            }

            public T设置吹膜机组开机前确认项目()
            {
                tblName = "设置吹膜机组开机前确认项目";
                cols = new Cols();
            }
        }

		public class T设置吹膜机组预热参数记录表 : Table
        {
             public class Cols:BaseCols
            {
                //public String 生产指令编号;
                //public Int32 生产指令id;
                //public DateTime 日期;
                //public Int32 记录人id;
                //public String 记录人;
                //public Int32 审核人id;
                //public String 审核人;
                //public String 审核意见;
                //public Boolean 审核是否通过;
                //public Double 模芯规格参数1;
                //public Double 模芯规格参数2;
                public Double 温度公差;
                //public DateTime 预热开始时间;
                //public DateTime 保温结束时间1;
                //public DateTime 保温开始时间;
                //public DateTime 保温结束时间2;
                //public DateTime 保温结束时间3;
                public Double 换网预热参数设定1;
                public Double 流道预热参数设定1;
                public Double 模颈预热参数设定1;
                public Double 机头1预热参数设定1;
                public Double 机头2预热参数设定1;
                public Double 口模预热参数设定1;
                public Double 一区预热参数设定1;
                public Double 二区预热参数设定1;
                public Double 三区预热参数设定1;
                public Double 四区预热参数设定1;
                public Double 换网预热参数设定2;
                public Double 流道预热参数设定2;
                public Double 模颈预热参数设定2;
                public Double 机头1预热参数设定2;
                public Double 机头2预热参数设定2;
                public Double 口模预热参数设定2;
                public Double 一区预热参数设定2;
                public Double 二区预热参数设定2;
                public Double 三区预热参数设定2;
                public Double 四区预热参数设定2;
                public Double 加热保温时间1;
                public Double 加热保温时间2;
                public Double 加热保温时间3;
                //public String 备注;

            }
            public T设置吹膜机组预热参数记录表()
            {
                cols = new Cols();
                tblName = "设置吹膜机组预热参数记录表";
            }
        }

        public class T设置吹膜工艺 : Table
        {
            public class Cols : BaseCols
            {
                public String 工艺名称;
            }

            public T设置吹膜工艺()
            {
                tblName = "设置吹膜工艺";
                cols = new Cols();
            }
        }
		
        //B下拉菜单
        public class T产品内包装记录 : Table
        {
            public class Cols:BaseCols
            {
                public Int32 生产指令ID;
                public String 产品代码;
                //public Int32 产品代码id;
                public String 产品批号;
                //public Int32 产品批号id;
                public DateTime 生产日期;
                //public Int32 序号;
                //public DateTime 生产时间;
                //public Int32 内包序号;
                //public Int32 产品外观;//界面：符合标准填写“√”，不符合填写“×”，不适用填写“N”
                //                      //数据库：符合标准填写0，不符合填写1，不适用填写2
                //public Int32 包装后外观;//同产品外观
                //public Int32 包装袋热封线;//同产品外观
                //public Int32 贴标签;//同产品外观
                //public Int32 贴指示剂;//同产品外观
                //public Int32 包装人id;             
                public String 包材名称;
                public String 包材批号;
                public Double 包材接上班数量;
                public Double 包材领取数量;
                public Double 包材剩余数量;
                public Double 包材使用数量;
                public Double 包材退库数量;
                public String 指示剂批号;
                public Double 指示剂接上班数量;
                public Double 指示剂领取数量;
                public Double 指示剂剩余数量;
                public Double 指示剂使用数量;
                public Double 指示剂退库数量;
                public Int32 标签发放数量;
                public Int32 标签使用数量;
                public Int32 标签销毁数量;
                public Int32 包装规格;
                public Boolean 标签语言是否中文;
                public Boolean 标签语言是否英文;
                public Int32 总计包数;
                public Int32 每片只数;
                //public Int32 操作人id;
                public String 操作人;
                public DateTime 操作日期;
                //public Int32 审核人id;
                public String 审核人;
                public DateTime 审核日期;
                public String 审核意见;
                public Boolean 审核是否通过;
            }
            public T产品内包装记录()
            {
                cols = new Cols();
                tblName = "产品内包装记录表";
            }
        }

        public class T产品内包装详细记录 : Table
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 T产品内包装记录ID;
                public Int32 序号;
                public DateTime 生产时间;
                public Int32 内包序号;
                public Int32 产品外观;//界面：符合标准填写“√”，不符合填写“×”，不适用填写“N”
                //数据库：符合标准填写0，不符合填写1，不适用填写2
                public Int32 包装后外观;//同产品外观
                public Int32 包装袋热封线;//同产品外观
                public Int32 贴标签;//同产品外观
                public Int32 贴指示剂;//同产品外观
                public String 包装人;
            }

            public T产品内包装详细记录()
            {
                tblName = "产品内包装详细记录";
                cols = new Cols();
            }
        }

        public class T产品外包装记录 : Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID;
                 public String 产品代码;
                //public Int32 产品代码id;
                public String 产品批号;
                //public Int32 产品批号id;
                public DateTime 包装日期;
                //public Int32 序号;
                //public Int32 包装箱号;
                //public String 包装明细;
                //public Boolean 是否贴标签;
                //public Boolean 是否打开包封箱;
                public Int32 包装规格每包只数;
                public Double 包装规格每包重量;
                public Int32 包装规格每箱包数;
                public Double 包装规格每箱重量;
                public Int32 产品数量箱数;
                public Int32 产品数量只数;
                public Int32  包材用量箱数;
                public Int32  包材用量标签数量;
                public String 备注;
                //public Int32 操作人id;
                public String 操作人;
                public DateTime 操作日期;
                //public Int32 审核人id;
                public String 审核人;
                public DateTime 审核日期;
                public String 审核意见;
                public Boolean 审核是否通过;

            }
            public T产品外包装记录()
            {
                cols = new Cols();
                tblName = "产品外包装记录表";
            }
        }

        public class T产品外包装详细记录 : Table
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 T产品外包装记录ID;
                public Int32 序号;
                public Int32 包装箱号;
                public String 包装明细;
                public Boolean 是否贴标签_是;
                public Boolean 是否贴标签_否;
                public Boolean 是否打开包封箱_是;
                public Boolean 是否打开包封箱_否;
            }

            public T产品外包装详细记录()
            {
                tblName = "产品外包装详细记录";
                cols = new Cols();
            }
        }

        // 生产指令结束之前，每次打开日报表都去其他表里读数据，然后显示
        // 生产指令结束之后，写入数据
        // 去掉填报人和复核人吧，因为是自动出来的。
        public class T吹膜生产日报表 : Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID;
                 public String 生产指令;
                 public Double 生产数量合计;
                 public Double 生产重量合计;
                 public Double 废品重量合计;
                 public Double 加料A合计;
                 public Double 加料B1C合计;
                 public Double 加料B2合计;
                 public Double 工时合计;
                 public Double 中层B2物料占比;
                 public Double 工时效率;
                 public String 备注;
            }
            public T吹膜生产日报表()
            {
                cols = new Cols();
                tblName = "吹膜生产日报表";
            }
        }

        public class T吹膜生产日报表详细信息:Table
        {
            public class Cols : BaseCols
            {
                public Int32 序号;
                public DateTime 生产时间;
                public Boolean 班次;
                public String 产品代码;
                public String 产品批号;
                public String 卷号; // 哪里来的？ // 生产检验记录中的  模卷编号？
                public Double 生产数量;
                public Double 生产重量;
                public Double 废品重量;
                public Double 加料A;
                public Double 加料B1C;
                public Double 加料B2;
                public Double 工时;
                public String 填报人;
                public String 审核人;
            }

            public T吹膜生产日报表详细信息()
            {
                tblName = "吹膜生产日报表详细信息";
                cols = new Cols();
            }
        }

        public class T吹膜供料记录 : Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID;
                 public String 产品代码;
                 //public Int32 产品代码id;
                 public String 产品批号;
                 //public Int32 产品批号id;
                 public String 生产指令编号;
                 //public Int32 生产指令id;
                 public String 外中内层原料代码;
                 public String 中层原料代码;
                 public String 外中内层原料批号;
                 public String 中层原料批号;
                 public Double 外中内层原料用量;
                 public Double 外中内层原料余量;
                 public Double 中层原料用量;
                 public Double 中层原料余量;
                 //public Int32 审核人id;
                 public String 审核人;
                 public String 审核意见;
                 public Boolean 审核是否通过;
                 // 下面三个值在每次从界面存入改表时进行计算
                 // 累加本生产指令下，本产品代码的所有值。
                 public Double 外层供料量合计a;
                 public Double 中内层供料量合计b;
                 public Double 中层供料量合计c;
            }
            public T吹膜供料记录()
            {
                cols = new Cols();
                tblName = "吹膜供料记录";
            }
        }

        public class T吹膜供料记录详细信息 : Table   
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 T吹膜供料记录ID;
                public DateTime 供料时间;
                public Double 外层供料量;
                public Double 中内层供料量;
                public Double 中层供料量;
                public Boolean 原料抽查结果;
                //public Int32 供料人id;
                public String 供料人;
            }
            public T吹膜供料记录详细信息()
            {
                cols = new Cols();
                tblName = "吹膜供料记录详细信息";
            }
        }

        public class T吹膜工序生产和检验记录 : Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID;
                 public String 产品名称;
                 //public Int32 产品代码;
                 public String 产品批号;
                 //public Int32 产品批号id;
                 public DateTime 生产日期;
                 public Boolean 班次;
                 public String 依据工艺;
                 public String 生产设备;
                 public Double 环境温度;
                 public Double 环境湿度;
                 //public Int32 审核人id;
                 public String 审核人;
                 public String 审核意见;
                 public Boolean 审核是否通过;
                 public Double 累计同规格膜卷长度R;
                 public Double 累计同规格膜卷重量T;
            }
            public T吹膜工序生产和检验记录()
            {
                cols = new Cols();
                tblName = "吹膜工序生产和检验记录";
            }
        }

        public class T吹膜工序生产和检验记录详细信息 : Table  //通过产品批号id及班次连接到上表
        {
            public class Cols : BaseCols
            {
                // 外键
                public Int32 T吹膜工序生产和检验记录ID;
                //public Int32 产品批号id;
                public Int32 序号;
                public DateTime 开始时间;
                public DateTime 结束时间;
                //public Boolean 班次;
                //public DateTime 时间;
                public String 膜卷编号;
                public Double 膜卷长度;
                public Double 膜卷重量;
                //public Int32 记录人id;
                public String 记录人;
                public Boolean 外观_是;
                public Boolean 外观_否;
                public Double 宽度;
                public Double 最大厚度;
                public Double 最小厚度;
                public Double 平均厚度;
                public Double 厚度公差;
                //public Int32 检查人id;
                public String 检查人;
                public Boolean 判定_是;
                public Boolean 判定_否;

            }
            public T吹膜工序生产和检验记录详细信息()
            {
                cols = new Cols();
                tblName = "吹膜工序生产和检验记录详细信息";
            }
        }

        public class T吹膜工序废品记录 : Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID;
                 public String 生产指令;
                 public DateTime 生产开始时间;
                 public DateTime 生产结束时间;
                 //public Int32 审核人id;
                 public String 审核人;
                 public String 审核意见;
                 public Boolean 审核是否通过;
                 public Double 合计不良品数量;
            }
            public T吹膜工序废品记录()
            {
                cols = new Cols();
                tblName = "吹膜工序废品记录";
            }
        }

        public class T吹膜工序废品记录详细信息 : Table//通过废品记录ID与上表连接
        {
            public class Cols : BaseCols
            {
                public Int32 T吹膜工序废品记录ID;
                //public String 生产指令;
                //public Int32 生产指令id;
                public Int32 序号;
                public DateTime 生产日期;
                public Boolean 班次;
                public String 产品代码;
                //public Int32 产品代码id;
                public Double 不良品数量;
                public String 废品产生原因;
                //public Int32 记录人id;
                public String 记录人;
                //public Int32 单条记录审核人id;
                public String 审核人;
            }
            public T吹膜工序废品记录详细信息()
            {
                cols = new Cols();
                tblName = "吹膜工序废品记录详细信息";
            }
        }

        public class T吹膜工序物料平衡记录 : Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID ;
                public String 生产指令;
                public DateTime 生产日期;
                public Double 成品重量合计;
                public Double 废品量合计;
                public Double 领料量;
                public Double 重量比成品率;
                public Double 物料平衡;
                public String 记录人;
                public DateTime 记录日期;
                public String 审核人;
                public DateTime 审核日期;
                public String 审核意见;
                public Boolean 审核是否通过;
                public String 备注;

            }
            public T吹膜工序物料平衡记录()
            {
                cols = new Cols();
                tblName = "吹膜工序物料平衡记录";
            }
        }

        //public class T吹膜工序传料记录 : Table
        //{
        //     public class Cols:BaseCols
        //    {
        //       public DateTime 传料日期;
        //       public DateTime 传料时间;
        //       public String 物料代码;
        //       public Int32 数量;
        //       public Double Kg每件;
        //       public Double 数量每kg;
        //       public Boolean 包装是否完好;
        //       public Boolean 是否清洁合格;
        //       public String 操作人;
        //       public Int32 操作人id;
        //       public String 单条审核人;
        //       public Int32 单条审核人id;
        //       public String 审核人;
        //       public Int32 审核人id;
        //       public Boolean 审核是否合格;
        //       public String 审核意见;

        //    }
        //    public T吹膜工序传料记录()
        //    {
        //        cols = new Cols();
        //        tblName = "吹膜工序传料记录";
        //    }
        //}

        public class T吹膜工序领料退料记录 : Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID;
               public String 物料代码;
               //public Int32 物料代码id;
               public DateTime 领料日期;
               //public Double 数量;
               //public Double Kg每件;
               //public Double 数量每kg;
               //public Boolean 包装是否完好;
               //public Boolean 是否清洁合格;
               //public String 操作人;
               //public Int32 操作人id;
               //public String 单条审核人;
               //public Int32 单条审核人id;
               public String 审核人;
               //public Int32 审核人id;
               public String 审核意见;
               public Boolean 审核是否通过;
               public Double 重量合计;
               public Double 数量合计;
               public Double 退料;
               public String 退料操作人;
               public String 退料审核人;
            }
            public T吹膜工序领料退料记录()
            {
                cols = new Cols();
                tblName = "吹膜工序领料退料记录";
            }
        }

        public class T吹膜工序领料详细记录 : Table
        {
            public class Cols : BaseCols
            {
                public Int32 T吹膜工序领料退料记录ID;
                public DateTime 领料日期;
                public Int32 数量;
                public Double 重量每件;
                public Double 重量;
                public Boolean 包装完好_是;
                public Boolean 包装完好_否;
                public Boolean 清洁合格_是;
                public Boolean 清洁合格_否;
                public String 操作人;
                public String 审核人;
            }
            public T吹膜工序领料详细记录()
            {
                tblName = "吹膜工序领料详细记录";
                cols = new Cols();
            }
        }
       
    

        //public class T吹膜工序退料记录 : Table//通过物料代码与上表连接
        //{
        //    public class Cols : BaseCols
        //    {
        //        public String 物料代码;
        //        public Int32 物料代码id;
        //        public Double 数量合计;
        //        public Double Kg每件合计;
        //        public Double 数量每kg合计;
        //        public Double 退料数量合计;
        //        public String 退料操作人;
        //        public Int32 退料操作人id;
        //        public String 退料审核人;
        //        public Int32 退料审核人id;
        //        public String 审核意见;
        //        public Boolean 审核是否通过;
        //    }
        //    public T吹膜工序退料记录()
        //    {
        //        cols = new Cols();
        //        tblName = "吹膜工序退料记录";
        //    }
        //}

        //public class T吹膜标签 : Table
        //{
        //     public class Cols:BaseCols
        //    {
        //         public String 膜代码;
        //         public String 批号及卷号;
        //         public Double 数量米;
        //         public Double 数量kg;
        //         public Boolean 班次;
        //         public String 备注;
        //    }
        //    public T吹膜标签()
        //    {
        //        cols = new Cols();
        //        tblName = "吹膜标签";
        //    }
        //}

        //C下拉菜单
        public class T吹膜机组开机前确认表 : Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID;
                 //public Int32 开机前确认ID;
                 //public String 生产指令编号;
                 //public Int32 确认人id;
                 public String 确认人;
                 public DateTime 确认日期;
                 //public Int32 审核人id;
                 public String 审核人;
                 public DateTime 审核日期;
                 public String 审核意见;
                 public Boolean 审核是否通过;
            }
            public T吹膜机组开机前确认表()
            {
                cols = new Cols();
                tblName = "吹膜机组开机前确认表";
            }
        }

        public class T吹膜机组开机前确认项目记录 : Table//通过开机前确认ID与上表连接
        {
            public class Cols : BaseCols
            {
                public Int32 T吹膜机组开机前确认表ID;
                public Int32 序号;
                public String 确认项目;
                public String 确认内容;
                public Boolean 确认结果_是;
                public Boolean 确认结果_否;
            }
            public T吹膜机组开机前确认项目记录()
            {
                cols = new Cols();
                tblName = "吹膜机组开机前确认项目记录";
            }
        }

        public class T吹膜机组预热参数记录表 : Table
        {
             public class Cols:BaseCols
            {
                public String 生产指令编号;
                public Int32 生产指令id;
                public DateTime 日期;
              //  public Int32 记录人id;
                public String 记录人;
              //  public Int32 审核人id;
                public String 审核人;
                public String 审核意见;
                public Boolean 审核是否通过;
                public Double 模芯规格参数1;
                public Double 模芯规格参数2;
             //   public Double 温度公差;
                public DateTime 预热开始时间;
                public DateTime 保温结束时间1;
                public DateTime 保温开始时间;
                public DateTime 保温结束时间2;
                public DateTime 保温结束时间3;
                //public Double 换网预热参数设定1;
                //public Double 流道预热参数设定1;
                //public Double 模颈预热参数设定1;
                //public Double 机头1预热参数设定1;
                //public Double 机头2预热参数设定1;
                //public Double 口模预热参数设定1;
                //public Double 一区预热参数设定1;
                //public Double 二区预热参数设定1;
                //public Double 三区预热参数设定1;
                //public Double 四区预热参数设定1;
                //public Double 换网预热参数设定2;
                //public Double 流道预热参数设定2;
                //public Double 模颈预热参数设定2;
                //public Double 机头1预热参数设定2;
                //public Double 机头2预热参数设定2;
                //public Double 口模预热参数设定2;
                //public Double 一区预热参数设定2;
                //public Double 二区预热参数设定2;
                //public Double 三区预热参数设定2;
                //public Double 四区预热参数设定2;
                //public Double 加热保温时间1;
                //public Double 加热保温时间2;
                //public Double 加热保温时间3;
                public String 备注;

            }
            public T吹膜机组预热参数记录表()
            {
                cols = new Cols();
                tblName = "吹膜机组预热参数记录表";
            }
        }
			
        //public class T挤出机预热参数记录表 : Table
        //{
        //     public class Cols:BaseCols
        //    {

        //    }
        //    public T挤出机预热参数记录表()
        //    {
        //        cols = new Cols();
        //        tblName = "挤出机预热参数记录表";
        //    }
        //}

        public class T吹膜供料系统运行记录 : Table
        {
             public class Cols:BaseCols
            {
                public String 生产指令编号;
                public Int32 生产指令ID;
				public DateTime 生产日期;
                public Boolean 班次;
				public String 审核人;
                //public Int32 审核人id;
                public String 审核意见;
                public Boolean 审核是否通过;
            }
            public T吹膜供料系统运行记录()
            {
                cols = new Cols();
                tblName = "吹膜供料系统运行记录";
            }
        }

        public class T吹膜供料系统运行记录详细信息 : Table
        {
            public class Cols : BaseCols
            {               
                public Int32 T吹膜供料系统运行记录ID;
                
                public DateTime 检查时间;
                public String 电机工作是否正常;
                public String 气动阀工作是否正常;
                public String 供料运行是否正常;
                public String 有无警报显示;
                public String 是否解除警报;
                public String 检查人;
              //  public Int32 检查人id;


            }
            public T吹膜供料系统运行记录详细信息()
            {
                cols = new Cols();
                tblName = "吹膜供料系统运行记录详细信息";
            }
        }

        public class T吹膜机组运行记录: Table
        {
             public class Cols:BaseCols
            {
                 public Int32 生产指令ID;
                 public String 产品代码;
                 //public Int32 产品代码ID;
                 public String 产品批号;
                 //public Int32 产品批号ID;
                 public DateTime 生产日期;
                 public DateTime 记录时间;
                 public String 记录人;
                 //public Int32 记录人id;
                 public String 审核人;
                 //public Int32 审核人id;
                 public String 审核意见;
                 public Boolean 审核是否通过;
                 public Double A层一区实际温度;
                 public Double A层二区实际温度;
                 public Double A层四区实际温度;
                 public Double A层换网实际温度;
                 public Double A层流道实际温度;
                 public Double B层一区实际温度;
                 public Double B层二区实际温度;
                 public Double B层三区实际温度;
                 public Double B层四区实际温度;
                 public Double B层换网实际温度;
                 public Double B层流道实际温度;
                 public Double C层一区实际温度;
                 public Double C层二区实际温度;
                 public Double C层三区实际温度;
                 public Double C层四区实际温度;
                 public Double C层换网实际温度;
                 public Double C层流道实际温度;
                 public Double 模头模颈实际温度;
                 public Double 模头一区实际温度;
                 public Double 模头二区实际温度;
                 public Double 模头口模实际温度;
                 public Double 模头线速度;
                 public Double 模头流道实际温度;
                 public Double 第一牵引设置频率;
                 public Double 第一牵引实际频率;
                 public Double 第一牵引电流;
                 public Double 第二牵引设置频率;
                 public Double 第二牵引实际频率;
                 public Double 第二牵引设定张力;
                 public Double 第二牵引实际张力;
                 public Double 第二牵引电流;
                 public Double 外表面电机设置频率;
                 public Double 外表面电机实际频率;
                 public Double 外表面电机设定张力;
                 public Double 外表面电机实际张力;
                 public Double 外表面电机电流;
                 public Double 外冷进风机设置频率;
                 public Double 外冷进风机实际频率;
                 public Double 外冷进风机电流;
                 public Double A层下料口温度;
                 public Double B层下料口温度;
                 public Double C层下料口温度;
                 public Double 挤出机A层实际频率;
                 public Double 挤出机A层电流;
                 public Double 挤出机A层熔体温度;
                 public Double 挤出机A层前熔体;
                 public Double 挤出机A层后熔压;
                 public Double 挤出机A层螺杆转速;
                 public Double 挤出机B层实际频率;
                 public Double 挤出机B层电流;
                 public Double 挤出机B层熔体温度;
                 public Double 挤出机B层前熔体;
                 public Double 挤出机B层后熔压;
                 public Double 挤出机B层螺杆转速;
                 public Double 挤出机C层实际频率;
                 public Double 挤出机C层电流;
                 public Double 挤出机C层熔体温度;
                 public Double 挤出机C层前熔体;
                 public Double 挤出机C层后熔压;
                 public Double 挤出机C层螺杆转速;

            }
            public T吹膜机组运行记录()
            {
                cols = new Cols();
                tblName = "吹膜机组运行记录";
            }
        }

        //D下拉菜单
        public class T吹膜机安全培训记录 : Table
        {
            public class Cols:BaseCols
            {
                //public Int32 培训记录ID;
                public String 讲师;
                public String 培训地点;
                public DateTime 培训日期;
                public String 备注;
            }
            public T吹膜机安全培训记录()
            {
                cols = new Cols();
                tblName = "吹膜机安全培训记录";
            }

        }

        public class T吹膜机安全培训记录人员情况 : Table//通过培训记录id与上表连接
        {
            public class Cols : BaseCols
            {
                public Int32 T吹膜机安全培训记录ID;
                public Int32 序号;
                public String 部门;
               // public Int32 参与人员ID;
                public String 参与人员;
                public Boolean 是否需要参加;

            }
            public T吹膜机安全培训记录人员情况()
            {
                cols = new Cols();
                tblName = "吹膜机安全培训记录人员情况";
            }

        }

        public class T吹膜机组换模头检查表 : Table
        {
             public class Cols:BaseCols
            {
                 //public Int32 换模头ID;
                 public String 更换原因;
                 public String 更换前模头型号;
                 public DateTime 更换日期;
                 public String 更换后模头型号;
                 public String 检查人;
                 //public Int32 检查人id;
                 public DateTime 检查日期;
                 public String 审核人;
                 //public Int32 审核人id;
                 public DateTime 审核日期;
                 public String 审核意见;
                 public Boolean 审核是否通过;

            }
            public T吹膜机组换模头检查表()
            {
                cols = new Cols();
                tblName = "吹膜机组换模头检查表";
            }
        }

        public class T吹膜机组换模头检查项目 : Table//通过换模头id与上表连接
        {
            public class Cols : BaseCols
            {
                public Int32 换模头ID;
                public String 检查项目;
                public String 检查标准;
                public Boolean 检查结果_是;
                public Boolean 检查结果_否;
            }
            public T吹膜机组换模头检查项目()
            {
                cols = new Cols();
                tblName = "吹膜机组换模头检查项目";
            }
        }

        public class T吹膜机组换模芯检查表 : Table
        {
             public class Cols:BaseCols
            {
                //public Int32 换模芯id;
                public String 更换原因;
                public String 更换前模芯型号;
                public DateTime 更换日期;
                public String 更换后模芯型号;
                public String 检查人;
                //public Int32 检查人id;
                public DateTime 检查日期;
                public String 审核人;
                //public Int32 审核人id;
                public DateTime 审核日期;
                public String 审核意见;
                public Boolean 审核是否通过;
            }
            public T吹膜机组换模芯检查表()
            {
                cols = new Cols();
                tblName = "吹膜机组换模芯检查表";
            }
        }

        public class T吹膜机组换模芯检查项目 : Table//通过换模芯id与上表连接
        {
            public class Cols : BaseCols
            {
                public Int32 换模芯ID;
                public String 检查项目;
                public String 检查标准;
                public Boolean 检查结果_是;
                public Boolean 检查结果_否;
            }
            public T吹膜机组换模芯检查项目()
            {
                cols = new Cols();
                tblName = "吹膜机组换模芯检查项目";
            }
        }

        public class T吹膜机更换过滤网记录表 : Table
        {
             public class Cols:BaseCols
            {
                 public DateTime 更换日期;
                 public String 更换原因;
                 public Double 滤网目数层数;
                 public String 备注;
                 public String 更换人;
                 //public Int32 更换人id;
                 public String 审核人;
                 //public Int32 审核人id;
                 public String 审核意见;
                 public Boolean 审核是否通过;

            }
             public T吹膜机更换过滤网记录表()
            {
                cols = new Cols();
                tblName = "吹膜机更换过滤网记录表";
            }
        }



        //A下拉菜单
        public T批生产记录封面 t批生产记录封面;
        public T批生产记录产品详细信息 t批生产记录产品详细信息;
        public T批生产记录目录详细信息 t批生产记录目录详细信息;

        public T生产指令信息 t生产指令信息;
        public T生产指令产品列表 t生产指令产品列表;

        public T吹膜机组清洁记录 t吹膜机组清洁记录;
        public T吹膜机组清洁项目记录 t吹膜机组清洁项目记录;

        public T吹膜工序清场记录 t吹膜工序清场记录;
        public T吹膜工序清场供料工序项目记录 t吹膜工序清场供料工序项目记录;
        public T吹膜工序清场吹膜工序项目记录 t吹膜工序清场吹膜工序项目记录;

        public T吹膜岗位交接班记录 t吹膜岗位交接班记录;
        public T吹膜岗位交接班项目记录 t吹膜岗位交接班项目记录;


        //其他表
        public T设置吹膜产品 t设置吹膜产品;
        public T设置吹膜产品编码 t设置吹膜产品编码;
        public T设置物料代码 t设置物料代码;
        public T设置吹膜机组清洁项目 t设置吹膜机组清洁项目;
        public T设置岗位交接班项目 t设置岗位交接班项目;
        public T设置吹膜机组开机前确认项目 t设置吹膜机组开机前确认项目;
		public T设置吹膜机组预热参数记录表 t设置吹膜机组预热参数记录表;
        public T设置吹膜工艺 t设置吹膜工艺;

        //B下拉菜单
        public T产品内包装记录 t产品内包装记录;
        public T产品内包装详细记录 t产品内包装详细记录;

        public T产品外包装记录 t产品外包装记录;
        public T产品外包装详细记录 t产品外包装详细记录;

        public T吹膜生产日报表 t吹膜生产日报表;
        public T吹膜生产日报表详细信息 t吹膜生产日报表详细信息;

        public T吹膜供料记录 t吹膜供料记录;
        public T吹膜供料记录详细信息 t吹膜供料记录详细信息;

        public T吹膜工序生产和检验记录 t吹膜工序生产和检验记录;
        public T吹膜工序生产和检验记录详细信息 t吹膜工序生产和检验记录详细信息;

        public T吹膜工序废品记录 t吹膜工序废品记录;
        public T吹膜工序废品记录详细信息 t吹膜工序废品记录详细信息;

        public T吹膜工序物料平衡记录 t吹膜工序物料平衡记录;
        //public T吹膜工序传料记录 t吹膜工序传料记录;

        public T吹膜工序领料退料记录 t吹膜工序领料退料记录;
        public T吹膜工序领料详细记录 t吹膜工序领料详细记录;

        //public T吹膜工序退料记录 t吹膜工序退料记录;
        //public T吹膜标签 t吹膜标签;
        //C下拉菜单
        public T吹膜机组开机前确认表 t吹膜机组开机前确认表;
        public T吹膜机组开机前确认项目记录 t吹膜机组开机前确认项目记录;
        public T吹膜机组预热参数记录表 t吹膜机组预热参数记录表;
		
      //  public T挤出机预热参数记录表 t挤出机预热参数记录表;
        public T吹膜供料系统运行记录 t吹膜供料系统运行记录;
        public T吹膜供料系统运行记录详细信息 t吹膜供料系统运行记录详细信息;
        public T吹膜机组运行记录 t吹膜机组运行记录;
        //D下拉菜单
        public T吹膜机安全培训记录 t吹膜机安全培训记录;
        public T吹膜机安全培训记录人员情况 t吹膜机安全培训记录人员情况;
        public T吹膜机组换模头检查表 t吹膜机组换模头检查表;
        public T吹膜机组换模头检查项目 t吹膜机组换模头检查项目;
        public T吹膜机组换模芯检查表 t吹膜机组换模芯检查表;
        public T吹膜机组换模芯检查项目 t吹膜机组换模芯检查项目;
        public T吹膜机更换过滤网记录表 t吹膜机更换过滤网记录表;

        public Extrusion()
        {

            //A下拉菜单
            t批生产记录封面 = new T批生产记录封面();
            t批生产记录产品详细信息 = new T批生产记录产品详细信息();
            t批生产记录目录详细信息 = new T批生产记录目录详细信息();

            t生产指令信息 = new T生产指令信息();
            t生产指令产品列表 = new T生产指令产品列表();

            t吹膜机组清洁记录 = new T吹膜机组清洁记录();
            t吹膜机组清洁项目记录 = new T吹膜机组清洁项目记录(); 

            t吹膜工序清场记录 = new T吹膜工序清场记录();
            t吹膜工序清场供料工序项目记录 = new T吹膜工序清场供料工序项目记录();
            t吹膜工序清场吹膜工序项目记录 = new T吹膜工序清场吹膜工序项目记录();

            t吹膜岗位交接班记录 = new T吹膜岗位交接班记录();
            t吹膜岗位交接班项目记录 = new T吹膜岗位交接班项目记录();


            //其他表
            t设置吹膜产品 = new T设置吹膜产品();
            t设置吹膜产品编码 = new T设置吹膜产品编码();
            t设置物料代码 = new T设置物料代码();
            t设置吹膜机组清洁项目 = new T设置吹膜机组清洁项目();
            t设置岗位交接班项目 = new T设置岗位交接班项目();
            t设置吹膜机组开机前确认项目 = new T设置吹膜机组开机前确认项目();
			t设置吹膜机组预热参数记录表=new T设置吹膜机组预热参数记录表();
            t设置吹膜工艺 = new T设置吹膜工艺();

            //B下拉菜单
            t产品内包装记录 = new T产品内包装记录();
            t产品内包装详细记录 = new T产品内包装详细记录();

            t产品外包装记录 = new T产品外包装记录();
            t产品外包装详细记录 = new T产品外包装详细记录();

            t吹膜生产日报表 = new T吹膜生产日报表();
            t吹膜生产日报表详细信息 = new T吹膜生产日报表详细信息();

            t吹膜供料记录 = new T吹膜供料记录();
            t吹膜供料记录详细信息 = new T吹膜供料记录详细信息();


            t吹膜工序生产和检验记录 = new T吹膜工序生产和检验记录();
            t吹膜工序生产和检验记录详细信息 = new T吹膜工序生产和检验记录详细信息();

           t吹膜工序废品记录=new T吹膜工序废品记录();
           t吹膜工序废品记录详细信息 = new T吹膜工序废品记录详细信息();

           t吹膜工序物料平衡记录=new T吹膜工序物料平衡记录();

           //t吹膜工序传料记录=new T吹膜工序传料记录();

           t吹膜工序领料退料记录=new T吹膜工序领料退料记录();
           t吹膜工序领料详细记录 = new T吹膜工序领料详细记录();
           //t吹膜工序退料记录 = new T吹膜工序退料记录();

           //t吹膜标签=new T吹膜标签();
            //C下拉菜单
           t吹膜机组开机前确认表=new T吹膜机组开机前确认表();
           t吹膜机组开机前确认项目记录 = new T吹膜机组开机前确认项目记录();
           t吹膜机组预热参数记录表=new T吹膜机组预热参数记录表();
		   
         //  t挤出机预热参数记录表=new T挤出机预热参数记录表();
           t吹膜供料系统运行记录=new T吹膜供料系统运行记录();
           t吹膜供料系统运行记录详细信息 = new T吹膜供料系统运行记录详细信息();
           t吹膜机组运行记录=new T吹膜机组运行记录();
            //D下拉菜单
           t吹膜机安全培训记录=new T吹膜机安全培训记录();
           t吹膜机安全培训记录人员情况=new T吹膜机安全培训记录人员情况();
           t吹膜机组换模头检查表=new T吹膜机组换模头检查表();
           t吹膜机组换模头检查项目 = new T吹膜机组换模头检查项目();
           t吹膜机组换模芯检查表=new T吹膜机组换模芯检查表();
           t吹膜机组换模芯检查项目 = new T吹膜机组换模芯检查项目();
           t吹膜机更换过滤网记录表=new T吹膜机更换过滤网记录表();

        }


    }
}
