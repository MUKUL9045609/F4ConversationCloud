Create PROCEDURE [dbo].[sp_GetRoleDropdown]

as
begin 
select 
Id,
Name

FROM SuperAdminRoles  where IsActive ='1'

end