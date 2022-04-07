const uri = '/objects';
let todos = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addCodeTextbox = document.getElementById('add-code');
    const addValueTextbox = document.getElementById('add-value');
    
    var query = [{}];
    query[0][addCodeTextbox.value.trim()]=addValueTextbox.value.trim();

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(query)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addValueTextbox.value = '';
            addCodeTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function addItems() {
    const tb = document.getElementById('add-items');
    
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: tb.value.trim()
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            tb.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(code) {
    fetch(`${uri}/${code}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(code) {
    const item = todos.find(item => item.code === code);

    document.getElementById('edit-value').value = item.value;
    document.getElementById('edit-code').value = item.code;
}

function updateItem() {
    const itemId = document.getElementById('edit-code').value;
    const item = {
        code: parseInt(document.getElementById('edit-code').value.trim(), 10),
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'label';
        isCompleteCheckbox.value = item.code;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.code})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.code})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.value);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    todos = data;
}