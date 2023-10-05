import { getAuth, signOut } from "../../services/auth";

function Sidebar(){
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
          <div class = "btn nav-item" onClick={searchUsers}>
            <i class="bi bi-person-gear"></i>
          </div>
        </div>
      );
    }
  }

  function profileButton(){
    if(userRole != "Admin"){
      return(
        <div>
          <div class = "btn nav-item" onClick={profile}>
            <i class="bi bi-person-square"></i>
          </div>
        </div>
      );
    }
  }

  function searchDecksButton(){
    return(
      <div>
        <div class = "btn nav-item" onClick={searchDecks}>
          <i class="bi bi-search"></i>
        </div>
      </div>
    );
  }

  function signOutButton(){
    return(
      <div>
        <div class = "btn border-top outline-none" onClick={signOut}>
          <i class="bi bi-box-arrow-right"></i>
        </div>
      </div>
    );
  }

  function teacherRequestButton(){
    if(userRole === "Admin")
      return(
        <div>
          <div class = "btn nav-item" onClick={teacherRequest}>
            <i class="bi bi-person-lines-fill"></i>
          </div>
        </div>
      );
  }

  function categoriesButton(){
    if(userRole === "Admin")
      return(
        <div>
          <div class = "btn nav-item" onClick={categories}>
            <i class="bi bi-tags"></i>
          </div>
        </div>
      );
  }

  return(
    <div id="sidebar" class = "container-fluid fixed-top row" >
      <div id="sidebar-column" class = "d-flex flex-column justify-content-between col-auto bg-light" >
        <div>
          {searchUsersButton()}
          {teacherRequestButton()}
          {categoriesButton()}
          {profileButton()}
          {searchDecksButton()}      
              
        </div>

        <div>
          {signOutButton()}
        </div>            
      </div>
    </div> 
  );
}
export default Sidebar;