using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriticalTagSQLiteGen
{
    public class CriticalTagsStringTemplates
    {
        public const string SqlLiteTagGenSql = @"
-- Tag Name: {0}
-- Rig Number: {1}
-- Expression: {2}
-- Type: {3}
with ExistingSqlTagId as (SELECT MAX(SQLTAG_ID) as TagId FROM SQLTAG where NAME='{0}' and path='critical_tags'),
NextSqlTagIdInSeq as (SELECT MAX(val)+1 as TagId FROM SEQUENCES WHERE NAME='SQLTAG_SEQ'),
SqlTagIdToUse as (SELECT CASE WHEN a.TagId IS NULL THEN b.TagId ELSE a.TagId END AS TagId from ExistingSqlTagId a, NextSqlTagIdInSeq b),
ProviderToUse as (SELECT MIN(SQLTAGPROVIDER_ID) as ProviderId FROM SQLTAGPROVIDER)
INSERT OR REPLACE INTO SQLTAG(SQLTAG_ID,PROVIDERID,OWNERID,NAME,PATH,ENABLED,TAGTYPE,DATATYPE,ACCESSRIGHTS,SCANCLASS)
SELECT TagId, ProviderId, 0, '{0}', 'critical_tags', 1, 'DB', '{3}', 'Read_Write', 'Default' FROM SqlTagIdToUse, ProviderToUse ;

with ExistingSqlTagId as (SELECT MAX(SQLTAG_ID) as TagId FROM SQLTAG where NAME='{0}'),
NextSqlTagIdInSeq as (SELECT MAX(val) + 1 as TagId FROM SEQUENCES WHERE NAME='SQLTAG_SEQ'),
MaxTagId as (select CASE WHEN a.TagId > b.TagId THEN a.TagId ELSE b.TagId END as TagId FROM ExistingSqlTagId a, NextSqlTagIdInSeq b)
UPDATE SEQUENCES SET val = (select TagId from MaxTagId) WHERE NAME='SQLTAG_SEQ';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype, iscustom)
select sqltag_id, 'ExpressionType', 1, null, null, '', null, 0 from sqltag where name = '{0}' and path='critical_tags';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype, iscustom)
select sqltag_id, 'Expression', null, '{2}', null, '', null, 0  from sqltag where name = '{0}' and path='critical_tags';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype, iscustom)
select sqltag_id, 'HistoryMaxAge', 5, null, null, '', null, 0  from sqltag where name = '{0}' and path='critical_tags';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype, iscustom)
select sqltag_id, 'HistoryMaxAgeMode', 4, null, null, '', null, 0  from sqltag where name = '{0}' and path='critical_tags';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype, iscustom)
select sqltag_id, 'PrimaryHistoryProvider', null, 'critical_tags_endpoint', null, '', null, 0  from sqltag where name = '{0}' and path='critical_tags';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype, iscustom)
select sqltag_id, 'HistoricalScanclass', null, 'Default', null, '', null, 0  from sqltag where name = '{0}' and path='critical_tags';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype, iscustom)
select sqltag_id, 'HistoricalDeadband', null, null, 0.01, '', null, 0  from sqltag where name = '{0}' and path='critical_tags';

insert or replace into sqltagprop(tagid, name, intval, strval, doubleval, path, datatype, iscustom)
select sqltag_id, 'HistoryEnabled', 1, null, null, '', null, 0  from sqltag where name = '{0}' and path='critical_tags';";

        public const string SqlLiteScanClassCreateSql = @"
-- Create the scan class if necessary
with ExistingScanClasId as (SELECT MAX(SQLTSCANCLASS_ID) as ScanClassId FROM SQLTSCANCLASS where NAME='Default'),
NextScanClassIdInSeq as (SELECT MAX(val)+1 as ScanClassId FROM SEQUENCES WHERE NAME='SQLTSCANCLASS_SEQ'),
ScanClassIdToUse as (SELECT CASE WHEN a.ScanClassId IS NULL THEN b.ScanClassId ELSE a.ScanClassId END AS ScanClassId from ExistingScanClasId a, NextScanClassIdInSeq b),
ProviderToUse as (SELECT MIN(SQLTAGPROVIDER_ID) as ProviderId FROM SQLTAGPROVIDER)
INSERT OR REPLACE INTO SQLTSCANCLASS(SQLTSCANCLASS_ID, PROVIDERID, NAME, LORATE, HIRATE, DRIVINGTAGPATH, COMPARISON, COMPAREVALUE, MODE, STALETIMEOUT, EXECFLAGS, WRITETIMEOUT)
SELECT ScanClassId, ProviderId, 'Default', 10000, 10000, '', 'Equal', 0.0, 'Direct', 10000, 0, 0 FROM ScanClassIdToUse, ProviderToUse;

with ExistingScanClasId as (SELECT MAX(SQLTSCANCLASS_ID) as ScanClassId FROM SQLTSCANCLASS where NAME='Default'),
NextScanClassIdInSeq as (SELECT MAX(val)+1 as ScanClassId FROM SEQUENCES WHERE NAME='SQLTSCANCLASS_SEQ'),
MaxScanClassId as (select CASE WHEN a.ScanClassId  > b.ScanClassId  THEN a.ScanClassId  ELSE b.ScanClassId  END as ScanClassId  FROM ExistingScanClasId a, NextScanClassIdInSeq b)
UPDATE SEQUENCES SET val = (select ScanClassId from MaxScanClassId) WHERE NAME='SQLTSCANCLASS_SEQ';";

        public const string SqlLiteTagFolderCreateSql = @"
-- Create the Tag folder if necessary
-- Rig Number: {0}
with ExistingSqlTagId as (SELECT MAX(SQLTAG_ID) as TagId FROM SQLTAG where NAME='crtitical_tags'),
NextSqlTagIdInSeq as (SELECT MAX(val)+1 as TagId FROM SEQUENCES WHERE NAME='SQLTAG_SEQ'),
SqlTagIdToUse as (SELECT CASE WHEN a.TagId IS NULL THEN b.TagId ELSE a.TagId END AS TagId from ExistingSqlTagId a, NextSqlTagIdInSeq b),
ProviderToUse as (SELECT MIN(SQLTAGPROVIDER_ID) as ProviderId FROM SQLTAGPROVIDER)
INSERT OR REPLACE INTO SQLTAG(SQLTAG_ID,PROVIDERID,OWNERID,NAME,PATH,ENABLED,TAGTYPE,DATATYPE,ACCESSRIGHTS,SCANCLASS)
SELECT TagId, ProviderId, 0, 'critical_tags', '/', 1, 'Folder', 'Int4', 'Read_Write', 'Default' FROM SqlTagIdToUse, ProviderToUse;

with ExistingSqlTagId as (SELECT MAX(SQLTAG_ID) as TagId FROM SQLTAG where NAME='critical_tags'),
NextSqlTagIdInSeq as (SELECT MAX(val) + 1 as TagId FROM SEQUENCES WHERE NAME='SQLTAG_SEQ'),
MaxTagId as (select CASE WHEN a.TagId > b.TagId THEN a.TagId ELSE b.TagId END as TagId FROM ExistingSqlTagId a, NextSqlTagIdInSeq b)
UPDATE SEQUENCES SET val = (select TagId from MaxTagId) WHERE NAME='SQLTAG_SEQ';";


    }
}
