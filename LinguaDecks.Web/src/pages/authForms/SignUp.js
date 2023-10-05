import { Formik, Form, Field, ErrorMessage } from "formik";
import * as Yup from "yup";
import './Auth.css';
import { signUpAsync } from "../../services/auth";
import { useState } from "react";
import { Link } from "react-router-dom";

function SignIn() {
  const signInSchema = Yup.object().shape({
    name: Yup.string()
      .trim("Name can't have trailing whitespaces")
      .strict(true)
      .required("Name is required"),
    email: Yup.string()
      .email()
      .required("Email is required"),
    password: Yup.string()
      .trim("Password can't have trailing whitespaces")
      .strict(true)
      .required("Password is required")
      .min(8, "Password must be at least 8 characters long"),
    repeatPassword: Yup.string()
      .oneOf([Yup.ref('password'), null], "Passwords don't match")
      .required("Repeat is required"),
    role: Yup.string(),
    description: Yup.string()
      .notRequired()
      .when("role",
      {
        is: '1',
        then: () => 
          Yup.string()
          .trim("Description can't have trailing whitespaces")
          .strict(true)
          .required("Description is required")
          .max(300, "Description is too long")
      })
  });

  const initialValues = {name: '', email: '', password: '', repeatPassword: '', role: '0', description: ''};

  const [emailRegisteredError, setEmailRegisteredError] = useState('')

  async function handleSubmit(values) {
    try {
      setEmailRegisteredError('')
      await signUpAsync(values.name, values.email, values.password, parseInt(values.role), values.description);
    }
    catch (e) {
      if (e.response?.data.Code === 400) {
        setEmailRegisteredError('Email already registered')
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
          const { errors, touched, isValid, dirty, values } = formik
          return (
            <div class='w-25 align-self-center'>
              <h3 class='text-center mt-5 mb-4'>Sign Up</h3>
              <Form>
                <div class='mb-3'>
                  <Field type='text' name='name' class={'form-control'.concat(errors.name && touched.name ? " input-error" : '')} placeholder='Name'/>
                  <ErrorMessage name='name' component='span' class='error'/>
                </div>
                <div class='mb-3'>
                  <Field type='email' name='email' class={'form-control'.concat(errors.email && touched.email ? " input-error" : '')} placeholder='Email address'/>
                  <ErrorMessage name='email' component='span' class='error'/>
                  <span class={'error d-block'.concat(emailRegisteredError  ? " hidden" : '')}>{emailRegisteredError}</span>
                </div>
                <div class='mb-3'>
                  <Field type='password' name='password' class={'form-control'.concat(errors.password && touched.password ? " input-error" : '')} placeholder='Password'/>
                  <ErrorMessage name='password' component='span' class='error'/>
                </div>
                <div class='mb-3'>
                  <Field type='password' name='repeatPassword' class={'form-control'.concat(errors.repeatPassword && touched.repeatPassword ? " input-error" : '')} placeholder='Repeat password'/>
                  <ErrorMessage name='repeatPassword' component='span' class='error'/>
                </div>
                <p class='d-block text-center mb-2'>How do you want to use the platform?<br/>(Choose one)</p>
                <div class={'btn-group-vertical w-100'.concat(values.role === '1' ? ' mb-3' : ' mb-4')}>
                  <Field type="radio" class="btn-check" id="role-opt1" name="role" autocomplete="off" value='0'/>
                  <label class="btn btn-outline-primary btn-sm" for="role-opt1">I want to study languages and explore the platform</label>
                  <Field type="radio" class="btn-check" id="role-opt2" name="role" autocomplete="off" value='1'/>
                  <label class="btn btn-outline-primary btn-sm" for="role-opt2">I want to create decks and share them with others</label>
                </div>
                <div class={'mb-4'.concat(values.role !== '1' ? ' d-none' : '')}>
                  <p class='d-block text-center mb-2'>Tell us about yourself<br/>(300 characters max)</p>
                  <Field
                    as='textarea'
                    rows='4'
                    name='description'
                    class={'form-control'.concat(errors.description && touched.description ? " input-error" : '')}
                    placeholder='Description'/>
                  <ErrorMessage name='description' component='span' class='error'/>
                </div>
                <button type='submit' class='btn btn-sm btn-primary w-100 mb-3' disabled={!(dirty && isValid)}>Sign up</button>
                <div class='d-flex flex-row-reverse me-2'>
                  <p class='lh-sm text-center'>
                    <Link to='/signin' id='signInCreateAcc' class='link-underline link-underline-opacity-0 link-underline-opacity-75-hover'>
                      Already have an account?<br/>Sign in
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