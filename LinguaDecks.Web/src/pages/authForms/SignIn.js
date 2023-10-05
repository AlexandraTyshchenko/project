import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import './Auth.css';
import { signInAsync } from "../../services/auth";
import { useState } from "react";
import { Link } from "react-router-dom";

function SignIn() {
  const signInSchema = Yup.object().shape({
    email: Yup.string().email().required("Email is required"),
    password: Yup.string().required("Password is required")
  });

  const initialValues = {email: '', password: ''};

  const [signInError, setSignInError] = useState('')

  async function handleSubmit(values) {
    try {
      setSignInError('')
      await signInAsync(values.email, values.password);
    }
    catch (e) {
      if (e.response?.data.Code === 400 || e.response?.data.Code === 404) {
        setSignInError('Invalid email or password');
      }
      else {
        setSignInError('Internal server error');
      }
    }
  };

  return (
    <Formik
      initialValues={initialValues}
      validationSchema={signInSchema}
      onSubmit={(values) => {
        handleSubmit(values);
      }}>
      {(formik) =>
        {
          const { errors, touched, isValid, dirty } = formik
          return (
            <div class='w-25 align-self-center'>
              <h3 class='text-center mt-5 mb-4'>Sign In</h3>
              <Form>
                <div class='mb-3'>
                  <Field
                    type='email'
                    name='email'
                    class={'form-control'.concat(errors.email && touched.email  ? ' input-error' : '')}
                    placeholder='Email address'/>
                  <ErrorMessage name='email' component='span' class='error'/>
                </div>
                <div class='mb-4'>
                  <Field
                    type='password'
                    name='password'
                    class={'form-control'.concat(errors.password && touched.password ? ' input-error' : '')}
                    placeholder='Password'/>
                  <ErrorMessage name='password' component='span' class='error'/>
                </div>
                <button
                  type='submit'
                  class={'btn btn-sm btn-primary w-100'.concat(signInError ? ' mb-2' : ' mb-3')}
                  disabled={!(dirty && isValid)}>
                    Sign in
                </button>
                <span class={'error text-center mb-2'.concat(signInError ? ' d-block' : ' d-none')}>{signInError}</span>
                <div class='d-flex flex-row-reverse me-2'>
                  <p class='lh-sm text-center'>
                    <Link to='/signup' id='signInCreateAcc' class='link-underline link-underline-opacity-0 link-underline-opacity-75-hover'>
                      New to the platform?<br/>Create an account
                    </Link>
                  </p>
                </div>
              </Form>
            </div>
          );
        }
      }
    </Formik>
  );
}

export default SignIn