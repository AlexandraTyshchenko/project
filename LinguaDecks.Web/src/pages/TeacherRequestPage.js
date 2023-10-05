import React, { useEffect, useState } from "react";
import TeacherRequest from "../components/teacherRequest/TeacherRequest";
import Pagination from "../components/Pagination/Pagination";
import TeacherRequestService from "../services/TeacherRequestService";
import { Dropdown } from "react-bootstrap";
import { getAuth } from "../services/auth";
import axios from "axios";
function TeacherRequestPage() {
    const pageSize = 10;
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [isLoading, setIsLoading] = useState(true);
    const [teacherRequests, setTeacherRequests] = useState([]);
    const [error, setError] = useState(null);
    const [status, setStatus] = useState(2);
    const [update, setUpdate] = useState(true);
    const [statusMessage, setStatusMessage] = useState("Pending");
    const token = getAuth().accessToken;
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    useEffect(() => {
       
        async function fetchData() {
            try {
                setIsLoading(true);
                const responseData = await TeacherRequestService.getAll(
                    currentPage,
                    pageSize,
                    status
                );
                setTotalPages(Math.ceil(responseData.totalTeacherRequests / pageSize));
                setTeacherRequests(responseData.teacherRequests);
            } catch (error) {
                setError(error.message);
                console.error('Error fetching data:', error);
            } finally {
                setIsLoading(false);
            }
        }
        fetchData();
    }, [currentPage, status, update]);

    const updatePage = () => setUpdate(!update);

    const paginate = (pageNumber) => setCurrentPage(pageNumber);

    return (
        <div>
            <Pagination paginate={paginate} totalPages={totalPages} currentPage={currentPage} />
            <div className="d-flex">
                <h5 style={{ marginRight: '20px' }}>Request status</h5>
                <Dropdown>
                    <Dropdown.Toggle variant="light" id="language-dropdown">
                        {statusMessage}
                    </Dropdown.Toggle>
                    <Dropdown.Menu>
                        <Dropdown.Item onClick={() => { setStatus(0); setCurrentPage(1); setStatusMessage("Approved") }}>
                            Approved
                        </Dropdown.Item>
                        <Dropdown.Item onClick={() => { setStatus(1); setCurrentPage(1); setStatusMessage("Rejected") }}>
                            Rejected
                        </Dropdown.Item>
                        <Dropdown.Item onClick={() => { setStatus(2); setCurrentPage(1); setStatusMessage("Pending") }}>
                            Pending
                        </Dropdown.Item>
                    </Dropdown.Menu>
                </Dropdown>
            </div>

            <div className="container cont">
                <div className="row   fw-bold">
                    <div className="col-3 justify-content-center">Email</div>
                    <div className="col-3">Name</div>
                    <div className="col-3">Description</div>
                </div>
            </div>
            {isLoading ? (
                <div className="d-flex justify-content-center " style={{ height: '100vh' }}>
                    <div className="spinner-border" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                </div>
            ) : error ? (
                <div className="d-flex justify-content-center">
                    {error}
                </div>
            ) : teacherRequests.length === 0 ? (
                <div className="d-flex justify-content-center">
                    Nothing not found
                </div>
            ) : (
                teacherRequests.map((request, index) => (
                    <TeacherRequest
                        key={index}
                        UserId={request.userId}
                        Email={request.email}
                        Name={request.name}
                        Description={request.description}
                        Status={request.status}
                        update={updatePage}
                    />
                ))
            )}
        </div>
    );
}

export default TeacherRequestPage;
