﻿using System.Collections.Generic;
using Starcounter.Authorization.Core;
using Starcounter.Authorization.Database;

namespace Starcounter.Authorization.Helper
{
    public static class PermissionHelper
    {
        public static IEnumerable<PermissionSomebodyGroup> GetAllPsgForPermission(Permission permission)
        {
            return GetAllPsgForPermission(PermissionToken.GetForPermissionOrNull(permission));
        }

        public static IEnumerable<PermissionSomebodyGroup> GetAllPsgForPermission(PermissionToken permissionToken)
        {
            return Db.SQL<PermissionSomebodyGroup>(
                $"select a from {typeof(PermissionSomebodyGroup).FullName} a where a.{nameof(PermissionSomebodyGroup.Permission)} = ?",
                permissionToken);
        }

        public static void CloneSomebodyGroupAssignments(Permission fromPermission, Permission toPermission)
        {
            foreach (var group in GetAllPsgForPermission(fromPermission))
            {
                new PermissionSomebodyGroup
                {
                    Permission = PermissionToken.GetForPermissionOrCreate(toPermission),
                    Group = group.Group
                };
            }
        }
    }
}