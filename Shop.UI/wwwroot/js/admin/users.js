var app = new Vue({
    el: '#app',
    data:{
        username: "",
        password: "",
        users: [],
        loading: false,
    },
    mounted() {
        this.getUsers();
    },
    methods: {
        getUsers(){
            this.loading = true;
            axios.get('/users')
                .then(res => {
                    this.users = res.data
                    console.log(res)
                })
                .catch(err => {
                    console.log(err);
                })
        },
        createUser(){
            this.loading = true;
            axios.post('/users', {username: this.username, password: this.password})
                .then(res => {
                    console.log(res.data)
                    this.users.push(res.data)
                })
                .catch(err => {
                    console.log(err);
                })
        },
        deleteUser(userId, index){
            this.loading = true;
            axios.delete('/users/' + userId)
                .then(res => {
                    console.log(res.data)
                    if(res.data.succeeded) {
                        this.users.splice(index, 1);
                    }
                })
                .catch(err => {
                    console.log(err);
                })
                .then(() => {
                    this.loading = false;
                });
        }
    }
})