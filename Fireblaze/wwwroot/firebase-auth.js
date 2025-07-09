// Funzione per login
window.firebaseLogin = async function(email, password) {
    try {
        const result = await firebase.auth().signInWithEmailAndPassword(email, password);
        return result.user.email;
    } catch (error) {
        return `ERROR:${error.message}`;
    }
};

// Controlla se c'è un utente loggato
window.isUserLoggedIn = function() {
    return !!firebase.auth().currentUser;
};

// Ottieni l'email dell’utente loggato
window.getUserEmail = function() {
    return firebase.auth().currentUser?.email || null;
};

// Logout
window.firebaseLogout = function() {
    return firebase.auth().signOut();
};
