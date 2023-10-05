import React, { useState } from 'react';
import './TeacherRequest.css';
import TeacherRequestService from '../../services/TeacherRequestService';
import TeacherRequestModal from '../admin-modal-forms/TeacherRequestModal';
import { getAuth } from '../../services/auth';
import axios from 'axios';
function TeacherRequest(props) {
    const token = getAuth().accessToken;
axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    const [rejectModal, setRejectModal] = useState(false);
    const [acceptModal, setAcceptModal] = useState(false);
   
    async function Answer(answer) {
        try {
            await TeacherRequestService.answerRequest(props.UserId, answer);
            console.log(props.UserId);
            props.update();
        } catch (error) {
            console.log(error.message);
        }
    }

    function handleAcceptRequest() {
        Answer(true);
        setAcceptModal(false);
    }

    function handleRejectRequest() {
        Answer(false);
        setRejectModal(false);
    }

    return (
        <div>
            <div className="container cont">
                <div className="row justify-content-md-center request fw-bold">
                    <div className="col-3 justify-content-center">{props.Email}</div>
                    <div className="col-3">{props.Name}</div>
                    <div className="col-3">{props.Description}</div>
                    <div className="col-3 ">
                        <div className='d-flex justify-content-center'>
                            {props.Status === 2 ? (
                                <>
                                    <p className='tick button' onClick={() => setAcceptModal(true)}>&#10004;</p>
                                    <p className='cross button' onClick={() => setRejectModal(true)}>&#10006;</p>
                                </>
                            ) : props.Status === 0 ? (
                                <p className='cross button' onClick={() => setRejectModal(true)}>&#10006;</p>
                            ) : props.Status === 1 ? (
                                <p className='tick button ' onClick={() => setAcceptModal(true)}>&#10004;</p>
                            ) : null}
                        </div>
                    </div>
                </div>
            </div>


            <TeacherRequestModal
                show={acceptModal}
                onHide={() => setAcceptModal(false)}
                title="Teacher request acception"
                message={`Do you want to accept the request of the user ${props.Name}`}
                onConfirm={handleAcceptRequest}
            />


            <TeacherRequestModal
                show={rejectModal}
                onHide={() => setRejectModal(false)}
                title="Teacher request rejection"
                message={`Do you want to reject the request of the user ${props.Name}`}
                onConfirm={handleRejectRequest}
            />
        </div>
    );
}

export default TeacherRequest;
