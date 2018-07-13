```csharp
# Data_Orm
lambda,map orm,(code first ,db first)
code firest ,db first ,cache for redis,map

in Application_Start method

//cache model
LambdaMap.InstanceProperties(AppDomain.CurrentDomain.GetAssemblies(), namespace, dll);

//chache map
LambdaMap.InstanceMap(db);


//web.config
<configSections>
    <section name="RedisConfig" type="Redis.Config.RedisConfig" />
    <section name="DataConfig" type="Data.Config.DataConfig,Data" />
</configSections>

  <RedisConfig WriteServerList="127.0.0.1:6379" ReadServerList="127.0.0.1:6379" MaxWritePoolSize="10" MaxReadPoolSize="50" />

  <DataConfig>
    <Oracle>
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="DbFirst" IsEncrypt="false" Key="OraTestDb" />
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="CodeFirst" IsEncrypt="false" Key="OraDb" />
    </Oracle>
    <MySql>
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="DbFirst" IsEncrypt="false" Key="MyTestDb" />
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="CodeFirst" IsEncrypt="false" Key="MyDb" />
    </MySql>
   <SqlServer>
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="DbFirst" IsEncrypt="false" Key="SqlTestDb" />
      <Add ConnStr="connstr" IsOutSql="true" IsOutError="true" DesignModel="CodeFirst" IsEncrypt="false" Key="SqlDb" />
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
              </dynamic>
            </select>
        </sqlMap>
        
//map.config
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="MapConfig" type="Data.Config.MapConfig,Data"/>
  </configSections>

  <MapConfig>
    <SqlMap>
      <Add File="map/map.xml"/>
    </SqlMap>
  </MapConfig>
</configuration>

 
 

```
