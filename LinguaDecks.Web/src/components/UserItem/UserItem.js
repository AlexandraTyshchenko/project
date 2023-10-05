import './UserItem.css';

function UserItem(props) {
  return (
    <div className="container ">
      <div className="row justify-content-md-center user-item text-uppercase fw-bold">
        <div className="col mr-3">{props.name}</div>
        <div className="col mr-3">{props.email}</div>
        <div className="col mr-3">{props.role}</div>
      </div>
    </div>
  );
}

export default UserItem;
