import { signOut } from '../../services/auth';
import profileIcon from '../../assets/default_user_avatar.png'
import './layout.css';

function Header(props){
  const user = props.user;
  const isAuthorized = props.authorized;
  const redirectToSignInPage = () => {
    window.location.href = "/signin";
  }

  const profile = () => {
    window.location.href = "/profile";
  }

  return(
    <header id="header">
      <nav class="navbar navbar-expand-md navbar-dark bg-primary fixed-top">
        {isAuthorized ? (
          <button id = "open-sidebar" class="btn btn-primary" type="button" data-bs-toggle="offcanvas" data-bs-target="#sidebar" aria-controls="sidebar" >
            <i class="bi bi-collection"></i>
          </button>
        ):(
          <div class="btn btn-primary">
            <i class="bi bi-collection"></i>
          </div>
        )}
        <a class="navbar-brand me-auto mb-2 mb-lg-0" href="/">LinguaDecks</a>
        
        <div>
          <button 
            type='button'
            id="login-btn"
            class={"btn btn-light".concat(isAuthorized ? " d-none" : "")}
            onClick={redirectToSignInPage}>
              Sign In
          </button>
          <div id="user-profile-dropdown" class={"dropdown open".concat(!isAuthorized ? " d-none" : "")}>
            <a class="btn btn-light dropdown-toggle" type="button" id="triggerId" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
              <img
                id="profile-avatar"
                src={user?.icon || profileIcon}
                class="rounded-circle"
                alt="Avatar"
                width="32"/>
              {user?.name}
            </a>
            <div class="dropdown-menu" aria-labelledby="triggerId">
              <a class="dropdown-item" onClick={profile} href="#">Profile</a>
              <a class="dropdown-item" onClick={signOut} href='#'>Logout</a>
            </div>
          </div>
        </div>
      </nav>
    </header>
  );
}
export default Header;