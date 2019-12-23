import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Login } from './components/Login';
import { CreateUser } from './components/CreateUser';
import { ProductAdmin } from './components/ProductAdmin';
import { ProductView } from './components/ProductView';
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route path='/login' component={Login} />
            <Route path='/create-user' component={CreateUser} />
            <Route path='/product-admin' component={ProductAdmin} />
            <Route path='/product-search' component={ProductView} />
      </Layout>
    );
  }
}
