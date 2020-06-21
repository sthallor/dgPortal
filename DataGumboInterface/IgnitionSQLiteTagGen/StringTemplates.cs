using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgnitionSQLiteTagGen
{
    public class StringTemplates
    {
        public const string SqlLiteTagGenSql = @"
-- Tag Name: {0}
-- Rig Number: {1}
-- Expression: {2}
-- Type: {3}
with ExistingSqlTagId as (SELECT MAX(SQLTAG_ID) as TagId FROM SQLTAG where NAME='{0}'),
NextSqlTagIdInSeq as (SELECT MAX(val)+1 as TagId FROM SEQUENCES WHERE NAME='SQLTAG_SEQ'),
SqlTagIdToUse as (SELECT CASE WHEN a.TagId IS NULL THEN b.TagId ELSE a.TagId END AS TagId from ExistingSqlTagId a, NextSqlTagIdInSeq b),
ProviderToUse as (SELECT MIN(SQLTAGPROVIDER_ID) as ProviderId FROM SQLTAGPROVIDER)
INSERT OR REPLACE INTO SQLTAG(SQLTAG_ID,PROVIDERID,OWNERID,NAME,PATH,ENABLED,TAGTYPE,DATATYPE,ACCESSRIGHTS,SCANCLASS)
SELECT TagId, ProviderId, 0, '{0}', 'Edge Nodes/DataGumboGroup/Rig{1}Node', 1, 'DB', '{3}', 'Read_Write', 'Class 10s' FROM SqlTagIdToUse, ProviderToUse ;

with ExistingSqlTagId as (SELECT MAX(SQLTAG_ID) as TagId FROM SQLTAG where NAME='{0}'),
NextSqlTagIdInSeq as (SELECT MAX(val) + 1 as TagId FROM SEQUENCES WHERE NAME='SQLTAG_SEQ'),
MaxTagId as (select CASE WHEN a.TagId > b.TagId THEN a.TagId ELSE b.TagId END as TagId FROM ExistingSqlTagId a, NextSqlTagIdInSeq b)
UPDATE SEQUENCES SET val = (select TagId from MaxTagId) WHERE NAME='SQLTAG_SEQ';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype,iscustom)
select sqltag_id, 'ExpressionType', 1, null, null, '', null, 0 from sqltag where name = '{0}';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype,iscustom)
select sqltag_id, 'Expression', null, '{2}', null, '', null, 0  from sqltag where name = '{0}';";

        public const string SqlLiteScanClassCreateSql = @"
-- Create the scan class if necessary
with ExistingScanClasId as (SELECT MAX(SQLTSCANCLASS_ID) as ScanClassId FROM SQLTSCANCLASS where NAME='Class 10s'),
NextScanClassIdInSeq as (SELECT MAX(val)+1 as ScanClassId FROM SEQUENCES WHERE NAME='SQLTSCANCLASS_SEQ'),
ScanClassIdToUse as (SELECT CASE WHEN a.ScanClassId IS NULL THEN b.ScanClassId ELSE a.ScanClassId END AS ScanClassId from ExistingScanClasId a, NextScanClassIdInSeq b),
ProviderToUse as (SELECT MIN(SQLTAGPROVIDER_ID) as ProviderId FROM SQLTAGPROVIDER)
INSERT OR REPLACE INTO SQLTSCANCLASS(SQLTSCANCLASS_ID, PROVIDERID, NAME, LORATE, HIRATE, DRIVINGTAGPATH, COMPARISON, COMPAREVALUE, MODE, STALETIMEOUT, EXECFLAGS, WRITETIMEOUT)
SELECT ScanClassId, ProviderId, 'Class 10s', 10000, 10000, '', 'Equal', 0.0, 'Direct', 10000, 0, 0 FROM ScanClassIdToUse, ProviderToUse;

with ExistingScanClasId as (SELECT MAX(SQLTSCANCLASS_ID) as ScanClassId FROM SQLTSCANCLASS where NAME='Class 10s'),
NextScanClassIdInSeq as (SELECT MAX(val)+1 as ScanClassId FROM SEQUENCES WHERE NAME='SQLTSCANCLASS_SEQ'),
MaxScanClassId as (select CASE WHEN a.ScanClassId  > b.ScanClassId  THEN a.ScanClassId  ELSE b.ScanClassId  END as ScanClassId  FROM ExistingScanClasId a, NextScanClassIdInSeq b)
UPDATE SEQUENCES SET val = (select ScanClassId from MaxScanClassId) WHERE NAME='SQLTSCANCLASS_SEQ';";

        public const string SqlLiteTagFolderCreateSql = @"
-- Create the Tag folder if necessary
-- Rig Number: {0}
with ExistingSqlTagId as (SELECT MAX(SQLTAG_ID) as TagId FROM SQLTAG where NAME='Rig{0}Node'),
NextSqlTagIdInSeq as (SELECT MAX(val)+1 as TagId FROM SEQUENCES WHERE NAME='SQLTAG_SEQ'),
SqlTagIdToUse as (SELECT CASE WHEN a.TagId IS NULL THEN b.TagId ELSE a.TagId END AS TagId from ExistingSqlTagId a, NextSqlTagIdInSeq b),
ProviderToUse as (SELECT MIN(SQLTAGPROVIDER_ID) as ProviderId FROM SQLTAGPROVIDER)
INSERT OR REPLACE INTO SQLTAG(SQLTAG_ID,PROVIDERID,OWNERID,NAME,PATH,ENABLED,TAGTYPE,DATATYPE,ACCESSRIGHTS,SCANCLASS)
SELECT TagId, ProviderId, 0, 'Rig{0}Node', 'Edge Nodes/DataGumboGroup', 1, 'Folder', 'Int4', 'Read_Write', 'Default' FROM SqlTagIdToUse, ProviderToUse;

with ExistingSqlTagId as (SELECT MAX(SQLTAG_ID) as TagId FROM SQLTAG where NAME='Rig{0}Node'),
NextSqlTagIdInSeq as (SELECT MAX(val) + 1 as TagId FROM SEQUENCES WHERE NAME='SQLTAG_SEQ'),
MaxTagId as (select CASE WHEN a.TagId > b.TagId THEN a.TagId ELSE b.TagId END as TagId FROM ExistingSqlTagId a, NextSqlTagIdInSeq b)
UPDATE SEQUENCES SET val = (select TagId from MaxTagId) WHERE NAME='SQLTAG_SEQ';";


    }
}
