"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    var user = document.getElementById("senderInput").value; 
    connection.invoke("SendMessageToAll", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
/*document.getElementById("sendButton").addEventListener("click", function (event) {

    var user = document.getElementById("senderInput").value;
    var message = document.getElementById("messageInput").value;
    var receiver = document.getElementById("receiverInput").value;
    if (receiver.length > 0) {
        //if the receiver is not empty, send private message to the receiver.
        connection.invoke("SendMessageToReceiver", user, receiver, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        //send message to all user.
        connection.invoke("SendMessageToAll", user, message).catch(function (err) {
            return console.error(err.toString());
        });
    }

    event.preventDefault();
});*/