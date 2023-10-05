import { getAuth, signOut } from "../../services/auth";

function Openbar(){
  const userRole = getAuth().user.role;

  const profile = () => {
    window.location.href = "/profile";
  }

  const searchDecks = () => {
    window.location.href = "/";
  }
  
  const searchUsers = () => {
    window.location.href = "/users";
  }

  const teacherRequest = () => {
    window.location.href = "/teacherrequests";
  }

  const categories = () => {
    window.location.href = "/categories";
  }


  function searchUsersButton(){
    if(userRole === "Admin"){
      return(
        <div>
          <i class="bi bi-person-gear"></i>
          <div class="btn" onClick={searchUsers}>Search Users</div>
        </div>
      );
    }
  }

  function profileButton(){
    if(userRole != "Admin"){
      return(
        <div>
          <i class="bi bi-person-square"></i>
          <div class="btn" onClick={profile}>Profile</div>
        </div>
      );
    }
  }

  function searchDecksButton(){
    return(
      <div>
        <i class="bi bi-search"></i>
        <div class="btn" onClick={searchDecks}>Search Decks</div>
      </div>
    );
  }

  function signOutButton(){
    return(
      <div class = "border-top">
          <i class="bi bi-box-arrow-right"></i>
          <div class="btn" onClick={signOut}>Sign out</div>
        </div>  
    );
  }

  function teacherRequestButton(){
    if(userRole === "Admin")
      return(
        <div>
          <i class="bi bi-person-lines-fill"></i>
          <div class = "btn" onClick={teacherRequest}>Teacher Requests</div>
        </div>
      );
  }

  function categoriesButton(){
    if(userRole === "Admin")
      return(
        <div>
          <i class="bi bi-tags"></i>
          <div class = "btn" onClick={categories}>Categories</div>
        </div>
      );
  }

  return(  
    <div class="offcanvas offcanvas-start" data-bs-backdrop="static" tabIndex="-1" id="sidebar" aria-labelledby="sidebarLabel">
      <div class="offcanvas-header">
        <h5 class="offcanvas-title" id="sidebarLabel">Lingua Decks</h5>
        <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
      </div>
      <div class="offcanvas-body">
        {searchUsersButton()}
        {teacherRequestButton()}
        {categoriesButton()}
        {profileButton()}
        {searchDecksButton()}    
        {signOutButton()}    
      </div>
    </div>  
  );
}
export default Openbar;