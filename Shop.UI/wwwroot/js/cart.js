var app = new Vue({
    el: '#app',
    data:{
        products: [],
    },
    mounted() {
        this.getCart()
    },
    methods: {
         addOneToCart(id) {
             let stockId = this.products[id].stockId;
             axios.post("/Cart/AddOne/" + stockId)
                .then(res => {
                    this.products[id].quantity += 1;
                })
                .catch(err => {
                    alert("Not enough stock");
                })
        },
        removeOneFromCart(id) {
            this.removeFromCart(id, 1);
        },
        removeAllFromCart(id) {
            this.removeFromCart(id, this.products[id].quantity);
        },
        removeFromCart(id, quantity) {
            let stockId = this.products[id].stockId;
            axios.post("/Cart/Remove/" + stockId + '/' + quantity)
                .then(res => {
                    this.products[id].quantity -= quantity;
                    if(this.products[id].quantity <= 0){
                        this.products.splice(id, 1);
                    }
                })
                .catch(err => {
                    alert(err.error);
                })
        },
        getCart() {
            axios.get('Cart/GetCart')
                .then(res => {
                    this.products = res.data;
                })
                .catch(err => {
                    console.log(err.message)
                })
        }
    },
})