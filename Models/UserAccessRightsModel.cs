namespace template.Models;

public class UserAccessRightsModel
{
    public int RoleID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // Assignments
    public bool AssignmentsAdd { get; set; } 
    public bool AssignmentsDelete { get; set; }
    public bool AssignmentsEdit { get; set; }
    public bool AssignmentsOpenRow { get; set; }
    
    // Customers
    public bool CustomersAdd { get; set; }
    public bool CustomersEdit { get; set; }
    public bool CustomersDelete { get; set; }
    
    // Users
    public bool UsersAdd { get; set; }
    public bool UsersEdit { get; set; }
    public bool UsersEditPassword { get; set; }
    public bool UsersEditAccessRole { get; set; }
    public bool UsersDelete { get; set; }
    
    // Gallery
    public bool GalleryAccess { get; set; }
    public bool GalleryDelete { get; set; }
    public bool GalleryAdd { get; set; }
    
    // Robot
    public bool RobotAdd { get; set; }
    public bool RobotEdit { get; set; }
    public bool RobotDelete { get; set; }
    
    // Comments
    public bool ViewInternalComments { get; set; }
    public bool ViewCustomerComments { get; set; }
    public bool EditCustomerComments { get; set; }
    
    // Sidebar access
    public bool SidebarAccess_WinterStorage { get; set; }
    public bool SidebarAccess_Customers { get; set; }
    public bool SidebarAccess_Users { get; set; }
    public bool SidebarAccess_Stores { get; set; }
    public bool SidebarAccess_Manufacturers { get; set; }
    public bool SidebarAccess_UserAccess { get; set; }
    public bool SidebarAccess_FileGallery { get; set; }
    public bool SidebarAccess_EmailSetup { get; set; }
    public bool RouteEdit { get; set; }
}