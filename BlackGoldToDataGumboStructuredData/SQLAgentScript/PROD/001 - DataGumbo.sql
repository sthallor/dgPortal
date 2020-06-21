DECLARE @ownerLoginName nvarchar(30) = dbo.eesOwnerLoginName();
DECLARE @jobName nvarchar(40) = N'DataGumbo' 
DECLARE @descript nvarchar(512) = '';

DECLARE @jobId binary(16)
SELECT @jobId = job_id FROM msdb.dbo.sysjobs WHERE (name = @jobName)
IF (@jobId IS NOT NULL)
BEGIN
    EXEC msdb.dbo.sp_delete_job @jobId
END

EXEC msdb.dbo.sp_add_job @job_name = @jobName, @description = @descript, @owner_login_name = @ownerLoginName, @notify_level_eventlog = 0
EXEC msdb.dbo.sp_add_jobstep @job_name = @jobName, @step_name = N'1', @step_id = 1, @subsystem = N'SSIS', @command = N'/FILE "\"\\cal0-pp-asis01\SSIS\DataGumbo\Package.dtsx\"" /CHECKPOINTING OFF /SET "\"\Package.Variables[User::intHoursBack].Properties[Value]\"";1 /SET "\"\Package.Variables[User::strAccessKey].Properties[Value]\"";"\"c8a56be2-b335-4e0d-968e-63d8f9924f7f\"" /SET "\"\Package.Variables[User::strApiKey].Properties[Value]\"";"\"670284b6-7773-4367-b85b-883d128f461c\"" /SET "\"\Package.Variables[User::strBGConnectionString].Properties[Value]\"";"\"Data Source=CAL0-PP-SQC1V10\PROD10;User ID=_blackGoldRead;Password=BUuXFUaB5H;Initial Catalog=Blackgold_PROD;Provider=SQLNCLI11.1;Auto Translate=False;\"" /SET "\"\Package.Variables[User::strServerName].Properties[Value]\"";"\"ensign-api-prd.azurewebsites.net\"" /SET "\"\Package.Variables[User::strLogFilePath].Properties[Value]\"";"\"\\CAL0-PP-ASIS01\SSIS\DataGumbo\Logs\"" /SET "\"\Package.Variables[User::strEnv].Properties[Value]\"";PROD /REPORTING E'
-- Hourly
EXEC msdb.dbo.sp_add_jobschedule @job_name = @jobName, @name=N'Every 15 minutes', 
		@enabled=1, @freq_type=4, @freq_interval=1, @freq_subday_type=4, 
		@freq_subday_interval=15, @freq_relative_interval=0, @freq_recurrence_factor=0, 
		@active_start_date=20190131, @active_end_date=99991231, 
		@active_start_time=0, @active_end_time=235959 
EXEC msdb.dbo.sp_add_jobserver @job_name=@jobName, @server_name = N'(local)'