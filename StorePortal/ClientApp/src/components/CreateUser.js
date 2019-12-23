import React, { Component } from 'react';
import { Form, Label, Input, Button, Text } from 'reactstrap';
import axios from 'axios';

export class CreateUser extends Component {
    static displayName = CreateUser.name;

    constructor(props) {
        super(props);
        this._isMounted = false;
        this.state = ({ Success: false, user: "", pass: "", status: "", logged: false });

    }

    componentDidMount() {
    }

    componentWillUnmount() {
    }

    checkCredentials = () => {
        //get user and pass from formd
        let form = new FormData();
        form.append('name', this.state.user);

        axios.post('auth', form)
            .then((result) => {
                let message = "Success!"
                result.data.Success ? this.setState({ status: message })
                    : this.setState({ status: "Fail" });
            })
            .catch((ex) => {
                console.error(ex);
            });

    }

    callCreate =  async() => {
            let form = new FormData();

            form.append('name', this.state.user);
            form.append('pass', this.state.pass);
             await axios.post('create', form)
                .then((result) => {
                    let message = "Success!"
                    result ? this.setState({ status: message, logged: true})
                        : this.setState({ status: "Fail" });
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

    getCredentials = async (user) => {
        this.setState({ user: user.target[0].value, pass: user.target[1].value, logged: true });
    }

    //changed API POST to button onClick onstead of form onSubmit
    render() {
        //Logged boolean test
        let results = this.state.logged ? "Account created" : "Awaiting input..";
        return (
            <div class ="topContent">
                <h3></h3>
                <Form>
                    <Label for="username">Username</Label>
                    <Input type="text" onChange={this.getUser.bind(this)} />
                    <Label for="password">Password</Label>
                    <Input type="password" onChange={this.getPass.bind(this)} />
                    <Button type="submit" onClick={this.callCreate}>Create account</Button>
                    <p>{results}</p>
                </Form>
            </div>
        );
    }
}
