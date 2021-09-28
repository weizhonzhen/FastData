```csharp
# Data_Orm
lambda,map orm,(code first ,db first)
code firest ,db first ,map
nuget url : https://www.nuget.org/packages/Fast.Data/
in Application_Start method

//aop by EventHandler
    public class TestAop : IFastAop
    {
        public void After(AfterContext context)
        {
           // throw new NotImplementedException();
        }

        public void Before(BeforeContext context)
        {
            //throw new NotImplementedException();
        }

        public void MapAfter(MapAfterContext context)
        {
            throw new NotImplementedException();
        }

        public void MapBefore(MapBeforeContext context)
        {
            throw new NotImplementedException();
        }
        
         public void Exception(Exception ex, string name)
         {
            throw new NotImplementedException();
         }
    }
    
//cache model
FastMap.InstanceProperties(namespace, "db.config",new TestAop());

//chache map
FastMap.InstanceMap(dbkey,"SqlMap.config","db.config",new TestAop());

//init map cache by Resource （xml file，SqlMap.config,db.config ,new TestAop()）
FastData.FastMap.InstanceMapResource(dbkey,"db.config","SqlMap.config",new TestAop());

//by Repository
 services.AddTransient<IFastRepository, FastRepository>();
 services.AddTransient<IRedisRepository, RedisRepository>();
 
 //redis by Resource db.config
 FastRedis.RedisInfo.Init("db.config")

interface  Service            
```csharp
    public interface TestService
    {
        [FastReadAttribute(dbKey = "Write", sql = "select * from TestResult where userId=?userId and userId=?userId")]
        List<Dictionary<string, object>> readListDic(string userId, string kid);

        [FastReadAttribute(dbKey = "Write", sql = "select * from TestResult where userId=?userId and userId=?userId")]
        Dictionary<string, object> readDic(string userId, string kid);

        [FastReadAttribute(dbKey = "Write", sql = "select * from TestResult where userId=?userId and userId=:userId")]
        List<TestResult> readModel(string userId, string kid);

        [FastReadAttribute(dbKey = "Write", sql = "select * from TestResult where userId=?userId and userId=?userId")]
        TestResult readListModel(string userId, string kid);

        [FastWriteAttribute(dbKey = "Write", sql = "update TestResult set userName=?userName where userId=?userId")]
        WriteReturn update(string userName, string userId);
    }

//ioc  
 var testService = FastMap.Resolve<TestService>();
 var write = testService.update("管理员", "admin");
 var readDic = testService.readDic("admin", "101");
 var readListDic = testService.readListDic("admin", "101");
 var readModel = testService.readModel("admin", "101");
 var readListModel = testService.readListModel("admin", "101");
            
 ```   

//web.config or db.config
<configSections>
    <section name="DataConfig" type="FastData.Config.DataConfig,FastData" />
</configSections>

 <DataConfig>
    <Oracle>
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="DbFirst" SqlErrorType="db" CacheType="web"  Key="OraTestDb" />
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="CodeFirst" SqlErrorType="file" CacheType="redis"  Key="OraDb" />
    </Oracle>
    <MySql>
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="DbFirst" SqlErrorType="db"  CacheType="web"  Key="MyTestDb" />
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="CodeFirst"  SqlErrorType="file" CacheType="redis"  Key="MyDb" />
    </MySql>
   <SqlServer>
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="DbFirst" SqlErrorType="db"  CacheType="web"  Key="SqlTestDb" />
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="CodeFirst"  SqlErrorType="file" CacheType="redis"  Key="SqlDb" />
   </SqlServer>
 </DataConfig>
 
 //map xml
        <?xml version="1.0" encoding="utf-8" ?>
          <sqlMap>
            <select id="GetUser" log="true">
              select a.*
              from base_user a
              <dynamic prepend=" where 1=1">
                <isPropertyAvailable prepend=" and " property="userId">a.userId=?userId</isPropertyAvailable>
                <isEqual compareValue="5" prepend=" and " property="userName">a.userName=?userName</isEqual>
                <isNotEqual compareValue="5" prepend=" and " property="fullName">a.fullName=?fullName</isNotEqual>
                <isGreaterThan compareValue="5" prepend=" and " property="orgId">a.orgId=?orgId</isGreaterThan>
                <isLessThan compareValue="5" prepend=" and " property="userNo">a.userNo=?userNo</isLessThan>
                <isNullOrEmpty prepend=" and " property="roleId">a.roleId=?roleId</isNullOrEmpty>
                <isNotNullOrEmpty prepend=" and " property="isAdmin">a.isAdmin=?isAdmin</isNotNullOrEmpty>
                <if condition="areaId>8" prepend=" and " property="areaId">a.areaId=?areaId</if>
                <if property="est" prepend=" and " condition="!FastUntility.Base.BaseRegular.IsZhString(#est#, false)" references="FastUntility">a=1</if>
                <choose property="userNo">
                   <condition prepend=" and " property="userNo>5">a.userNo=:userNo and a.userNo=5</condition>
                   <other prepend=" and ">a.userNo=:userNo and a.userNo=6</other> <!--by above 2.1.4-->
                </choose>
                <foreach name="data" field="userId" type="Test.Model.TestModel,Test">
                    select ypxh from base_role where userId=:userId
                </foreach>
              </dynamic>
            </select>
            
              <select id="Patient.Test">
                select * from base_user where 1=1
                <dynamic prepend="">
                  <isNotNullOrEmpty prepend=" and " property="userid">userid = :userid</isNotNullOrEmpty>
                </dynamic>
                <foreach name="data1" field="areaid" type="Test1.Model.BASE_AREA,Test1">
                  select * from base_area where areaid=:areaid
                </foreach>
                <foreach name="data2" field="roleid" type="Test1.Model.BASE_ROLE,Test1">
                  select * from base_role where roleid=:roleid
                </foreach>
              </select>
        </sqlMap>
        
        var param = new List<OracleParameter>();
        param.Add(new OracleParameter { ParameterName = "userid", Value = "dd5c99f2-0892-4179-83db-c2ccf243104c" });
        var tt = FastMap.Query<TestResult>("Patient.Test", param.ToArray(), null, "test");
        
        //Navigate
        var data = FastRead.Query<TestResult>(a => a.USERID != "" , null , "OraDb").toList<TestResult>();
        
         //Filter
        FastMap.AddFastFilter<TestResult>(a => a.USERID != "", FilterType.Query_Page_Lambda_Model);
        FastMap.AddFastFilter<BASE_AREA>(a => a.HOSPITALID != "", FilterType.Query_Page_Lambda_Model);
        
        var data1 = IFast.Query<TestResult>(a => a.ORGID == "1",null,"OraDb").ToPage<TestResult>(page);
        var data2 = IFast.Query<TestResult>(a => a.ORGID == "1",null,"OraDb").Filter(false).ToPage<TestResult>(page);
        
        namespace Test1.Model
        {
            public class TestResult
            {
                public string USERID { get; set; }
                public string USERPASS { get; set; }
                public string FULLNAME { get; set; }
                public string ORGID { get; set; }
                public string EXTENDORGID { get; set; }
                public string HOSPITALID { get; set; }
                public string EXTENDHOSPITALID { get; set; }
                public string AREAID { get; set; }
                public string EXTENDAREAID { get; set; }
                public string USERNO { get; set; }
                public string ROLEID { get; set; }
                public string EXTENDROLEID { get; set; }
                public string ISADMIN { get; set; }
                public string ISDEL { get; set; }
                public DateTime? ADDTIME { get; set; }
                public string ADDUSERID { get; set; }
                public string ADDUSERNAME { get; set; }
                public DateTime? DELTIME { get; set; }
                public string DELUSERID { get; set; }
                public string DELUSERNAME { get; set; }
                
                //Navigate
                public virtual List<BASE_AREA> area { get; set; }
                public virtual List<BASE_ROLE> role { get; set; }    
                
                [NavigateType(Type = typeof(BASE_ROLE))]
                public virtual List<Dictionary<string, object>> roleList { get; set; }
                                
                [NavigateType(Type = typeof(BASE_ROLE))]
                public virtual Dictionary<string, object> roleDic { get; set; }
            }
            
            public class BASE_ROLE
            {
                //Navigate
                [Navigate(Name = nameof(TestResult.ROLEID))]
                public string ROLEID{ get; set; }
                public string ROLENAME{ get; set; }
                public string ROLEREMARK{ get; set; }
                public string DEFAULTPAGE{ get; set; }
                public DateTime? ADDTIME{ get; set; }
                public string ADDUSERID{ get; set; }
                public string ADDUSERNAME{ get; set; }      
            }
            
            public class BASE_AREA
            {
                //Navigate
                [Navigate(Name = nameof(TestResult.AREAID))]
                public string AREAID{ get; set; }
                public string HOSPITALID{ get; set; }
                public string AREANAME{ get; set; }
                public DateTime? ADDTIME{ get; set; }
                public string ADDUSERID{ get; set; }
                public string ADDUSERNAME{ get; set; }
                public DateTime? DELTIME{ get; set; }
                public string DELUSERID{ get; set; }
                public string DELUSERNAME{ get; set; }
                public string ISDEL{ get; set; }      
            }
        }
        
//map.config
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="MapConfig" type="FastData.Config.MapConfig,FastData"/>
  </configSections>

  <MapConfig>
    <SqlMap>
      <Add File="map/map.xml"/>
    </SqlMap>
  </MapConfig>
</configuration>

 
 

```
