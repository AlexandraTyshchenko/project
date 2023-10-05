import React, { useState, useEffect } from 'react';
import UserItem from '../components/UserItem/UserItem';
import SearchService from '../services/SearchService';
import { Link } from 'react-router-dom';
import Pagination from '../components/Pagination/Pagination';
import SearchBar from '../components/SearchBar/SearchBarUsers';
import { getAuth } from '../services/auth';
import Loading from './Loading';
import axios from 'axios';

function ViewUsers() {
  const pageSize = 10;
  const [currentPage, setCurrentPage] = useState(1);
  const [userItems, setUserItems] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [isLoading, setIsLoading] = useState(true);
  const [name, setName] = useState("");
  const [email, setMail] = useState("");
  const [role, setRole] = useState("");
  const [error, setError] = useState(null);

  const token = getAuth().accessToken;
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

  useEffect(() => {
    async function fetchData() {
      try {
        setIsLoading(true);
        const responseData = await SearchService.getAllUsers(
          currentPage,
          pageSize,
          name,
          email,
          role,
        );
        setTotalPages(Math.ceil(responseData.totalUsers / pageSize));
        setUserItems(responseData.userItems);
      } 
      catch (error) {
        setError(error.message); 
        console.error('Error fetching data:', error);
      } 
      finally {
        setIsLoading(false);
      }
    }
    fetchData();
  }, [currentPage, name, email, role]);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  const search = (name, email, role) => {
    
    setName(name);
    setMail(email);
    setRole(role);
    setCurrentPage(1);
  };

  return (
    <div>
      <SearchBar search={search} />
      <div className='row justify-content-center'>        
        <Pagination className="col" paginate={paginate} totalPages={totalPages} currentPage={currentPage} />        
      </div>

      {isLoading ? <Loading /> : error ? (
        <div className="row justify-content-center">
          {error}
        </div>
      ) : userItems.length > 0 ? (
        userItems.map((user) => (
          
          /*<Link to={`/`} key={user.id}>*/
            <UserItem
              key={user.id}
              name={user.name}
              email={user.email}
              role={user.role}
            />
          /*</Link>*/
        ))
      ) : (
        <div className="d-flex justify-content-center ">
          <p>Nothing found</p>
        </div>
      )}
    </div>
  );
}

export default ViewUsers;
