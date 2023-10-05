import React, { useState, useEffect } from 'react';

function SearchBarUsers({search}){
  const [selectedRole, setSelectedRole] = useState("");
  const [name, setName] = useState("");
  const [mail, setMail] = useState("");
  
  function handleClick() {
    search(name, mail, selectedRole);
  }

  function Reset() {
    setSelectedRole("");
    setName("");
    setMail("");
    search("", "", "");
    document.getElementById("btn-name").textContent = "Roles";
  }

  function SelectRole(role){    
    setSelectedRole(role);  
  }

  document.querySelectorAll('.dropdown-item.role').forEach(function(item) {
    item.addEventListener('click', function() {
        document.querySelectorAll('.dropdown-item').forEach(function(element) {
            element.classList.remove('active');
        });
        this.classList.add('active');
        document.getElementById("btn-name").textContent = this.textContent;
    });
  });

  return(
    <div className='container'>
        <div className='row justify-content-center'>

          <div className='col-4 justify-content-center'>
            <input className="input form-control " type="text" placeholder="name" value={name} onChange={(event) => setName(event.target.value)} />
          </div>

          <div className='col-4 justify-content-center'>
            <input className="input form-control" type="text" placeholder="mail" value={mail} onChange={(event) => setMail(event.target.value)} />
          </div>

          <div className='col-2 justify-content-center'>
            <div class="dropdown">
              <button id = "btn-name" class="btn btn-info dropdown-toggle" type = "button" data-bs-auto-close="outside" data-bs-toggle="dropdown" aria-expanded="false">
                Roles
              </button>
              <ul class="dropdown-menu">
                <li><a class="dropdown-item role" onClick={()=>{SelectRole(0)}} type="button">Student</a></li>
                <li><a class="dropdown-item role" onClick={()=>{SelectRole(1)}}type="button">Teacher</a></li>
                <li><a class="dropdown-item role" onClick={()=>{SelectRole(2)}}type="button">Admin</a></li>

              </ul>
            </div>
          </div>

          <div className='col-1 justify-content-center'>
            <div className='btn btn-primary' onClick={handleClick}>Search</div>
          </div>
            
          <div className='col-1 justify-content-center'>
            <div className='btn btn-secondary' onClick={Reset}>Reset</div> 
          </div>
          

        </div>  
    </div>
  );
}

export default SearchBarUsers;