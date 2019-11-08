```csharp
# Data_Orm
lambda,map orm,(code first ,db first)
code firest ,db first ,map
nuget url : https://www.nuget.org/packages/Fast.Data/
in Application_Start method

//cache model
FastMap.InstanceProperties(namespace, dll);

//chache map
FastMap.InstanceMap(db);


//web.config
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
            
            <select id="Patient.NowAuditList">
                select cfsb,brxm from ms_cf01 where 1=1
            <dynamic prepend="">
              <isNotNullOrEmpty prepend=" and " property="brid">brid = :brid</isNotNullOrEmpty>
            </dynamic>
            <foreach name="data" field="cfsb" type="Test1.Model.MS_CF02,Test1">
              select ypxh from ms_cf02 where cfsb=:cfsb
            </foreach>
          </select>
        </sqlMap>
        
        var param = new List<OracleParameter>();
        param.Add(new OracleParameter { ParameterName = "brid", Value = "550010" });
        var tt = FastMap.Query<TestResult>("Patient.NowAuditList", param.ToArray(), null, "test");
        
        namespace Test1.Model
        {
            public class TestResult
            {
                public decimal? CFSB { get; set; }

                public string BRXM { get; set; }

                public List<MS_CF02> leaf { get; set; }
            }
            
            public class MS_CF02
            {
                public decimal? YPXH{ get; set; }
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
