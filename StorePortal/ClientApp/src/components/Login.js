import React, { Component } from 'react';
import axios from 'axios';
import { CreateUser } from './CreateUser';
import {ProductView} from './ProductView';
import { Form, Label, Input, Button, Text } from 'reactstrap';
import Table, { Thead, Tbody, Tr, Th, Td } from "react-row-select-table";

//Use this to render and display table depending on bool logged



/*=================== Bind user input to state ===========================*/

var getUser = (user) => {
    Login.user = user.target.value;
}

var getPass = (user) => {
    Login.pass = user.target.value;
}

var getKey = (user) => {
    Login.key = user.target.value;
}

/*========================== Render Methods =========================================*/
var renderSearch = () => {
    return (
            <Form>
                <h3>Product Search</h3>
                <Input type="text" onChange={getKey.bind(this)} />
                <Button type="button" onClick={search}>Search</Button>
            </Form>
    );
}

var renderProd = (products) => {
    return (
        <Table onCheck={value => console.log(value)} defaultCheckeds={[1, 3]}>
            <Thead>
                <Tr>
                    <Th>Name</Th>
                    <Th>Quantity</Th>
                    <Th>Price</Th>
                </Tr>
            </Thead>
            <Tbody>
                {products.map(prod =>
                    <Tr key={prod["name"]}>
                        <Td>{prod["name"]}</Td>
                        <Td>{prod["quantity"]}</Td>
                        <Td>{(prod["dprice"])}</Td>
                    </Tr>
                )}
            </Tbody>
        </Table>
    );
}

var renderLogin = () => {
    return (
        <Form>
            <Label for="userName">Username</Label>
            <Input type="text" onChange={getUser.bind(this)} />
            <Label for="password">Password</Label>
            <Input type="password" onChange={getPass.bind(this)} />
            <Button type="button" onClick={callLogin}>Apply</Button>
            <Button type="submit">Submit</Button>
            <Label></Label>
        </Form>
    );
}

var callLogin = async () => {
    let form = new FormData();
    form.append('name', Login.user);
    form.append('pass', Login.pass);

    await axios.post('login', form)
        .then((result) => {
            let message = "Success!"
            result.data ? Login.logged = true
                : Login.logged = false
        })

        .catch((ex) => {
            console.error(ex);
        });
}

var search = async () => {
    let form = new FormData();
    form.append('key', Login.key);

    await axios.post('search', form)
        .then((result) => {
            Login.products = result.data
        })
        .catch((ex) => {
            console.error(ex);
        });
}

/*====================== Login Class ===================================*/

export class Login extends Component{
    static displayName = Login.name;

    constructor(props) {
        super(props);
        this._isMounted = false;
        this.state = ({
            Success: false, user: "", pass: "",
            status: "",
            products: [], key: ""
        });

    }

    componentDidMount() {
        this.getProducts();
    }

    componentWillUnmount() {
    }

    search = async () => {
        let form = new FormData();
        form.append('key', this.state.key);

        await axios.post('search', form)
            .then((result) => {
                var array = [result.data]
                this.setState({
                    products: array
                });
            })
            .catch((ex) => {
                console.error(ex);
            });
    }

    getKey(key) {
        this.setState({
            key: key.target.value,
        });
    }

    callLogin = async() => {
            let form = new FormData();
            form.append('name', this.state.user);
            form.append('pass', this.state.pass);

            await axios.post('login', form)
                .then((result) => {
                    let message = "Success!"
                    result.data ? this.setState({ status: message, logged: true })
                        : this.setState({
                            status: "Incorrect username or password",
                            logged: false
                            });
                })
                .catch((ex) => {
                    console.error(ex);
                });
    }

    getUser(user) {
        this.setState({ user: user.target.value });
    }

    getPass(user) {
        this.setState({ pass: user.target.value });
    }

    render() {
        let render = this.state.logged ? renderProd(this.state.products) : "Please login to view products";
        let searchBar = this.state.logged ?
            <Form>
            <h3>Product Search</h3>
                <Input type="text" onChange={this.getKey.bind(this)} />
                <Button type="button" onClick={this.search}>Search</Button>
            </Form> : null;
        return (
            <div class="topContent">
                <h3></h3>
                <Form>
                    <Label for="userName">Username</Label>
                    <Input type="text" onChange={this.getUser.bind(this)}/>
                    <Label for="password">Password</Label>
                    <Input type="password" onChange={this.getPass.bind(this)}/>
                    <Button type="button" onClick={this.callLogin}>Login</Button>
                </Form>
                <div class="middleContent">
                    {searchBar}
                </div>  
                <div class ="bottomContent">
                    {render}
                </div>
            </div>
        
        //return (
        //    <div>
        //        {render}
        //    </div>
        );
    }

    async getProducts() {
        const response = await fetch('getProducts');
        const data = await response.json();
        this.setState({
            products: data
        });
    }
}
