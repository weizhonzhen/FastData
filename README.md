```csharp
# Data_Orm
lambda,map orm,(code first ,db first)
code firest ,db first ,map
nuget url : https://www.nuget.org/packages/Fast.Data/
in Application_Start method

//cache model
FastMap.InstanceProperties(namespace, "Test","db.config");

//chache map
FastMap.InstanceMap(db,"SqlMap.config","db.config");

//init map cache by Resource （xml file，SqlMap.config,db.config ）
FastData.FastMap.InstanceMapResource("Test","Test","db.config","SqlMap.config");

//by Repository
 services.AddTransient<IFastRepository, FastRepository>();
 services.AddTransient<IRedisRepository, RedisRepository>();
 
 //redis by Resource db.config
 FastRedis.RedisInfo.Init("Test","db.config")

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
            <select id="GetUser">
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
                <choose property="userNo">
                   <condition prepend=" and " property="userNo>5">a.userNo=:userNo and a.userNo=5</condition>
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
                public List<BASE_AREA> area { get; set; }
                public List<BASE_ROLE> role { get; set; }
            }
            
            public class BASE_ROLE
            {
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
