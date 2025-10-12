import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({

   container: {
        flex: 1,
        backgroundColor: theme.background
    },
    scrollView: {
        flex: 1,
    },
    containerConteudo: {
        paddingHorizontal: 20,
        paddingTop: 15,
        paddingBottom: 40,
    },
    nav: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
        paddingVertical: 15,
        marginBottom: 10,
        marginTop:15,
    },
    botaodevoltar: {
        width: 40,
        height: 40,
        justifyContent: 'center',
        marginTop: 10,
    },
    titulo: {
        fontSize: 18,
        color: theme.text,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        textAlign: 'center',
        flex: 1,
    },
    espacoHeader: {
        width: 30,
    },
    containerequipes: {
        padding: 5,
        marginBottom: 10,
        marginRight:50,
        flexDirection: 'row',
        alignItems: 'center',
        alignSelf:"center",
    },
    imagem: {   
        paddingVertical:5,
    },
    imagemequipe: {
        width: 70,
        height: 70,
        marginLeft: 10,
    },
    textos: {
        marginLeft: 15,
        flex: 1,
    },
    textoequipe: {
        color: theme.text,
        fontSize: 20,
        fontWeight: 'bold',
        fontFamily: fonts.text,
    },
    textoequipecargo: {
        color: theme.text,
        fontSize: 15,
        fontWeight: '300',
        fontFamily: fonts.text,
    },
    areaInput:{
        width: '100%',
        alignItems: 'center',
        paddingVertical:5,
    },
    texto: {
        fontSize: 15,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        color: theme.text,
        marginLeft:20,
        alignSelf: 'flex-start'
    },
    input: {
        width: 290,
        borderRadius: 6,
        borderWidth: 1,
        color:theme.text3,
        backgroundColor: theme.inputBackground,
        paddingVertical:8,    
        paddingHorizontal:20,
        marginBottom: 10,
        marginTop: 15,
    },
    containercategorias: {
        backgroundColor: theme.inputBackground,
        borderRadius: 10,
        padding: 10,
        marginBottom: 20,
        flexDirection: 'row',
        alignItems: 'center',
    }, 
    textolistatitulo: {
        color: theme.text,
        fontSize: 15,
        fontWeight: 'bold',
        fontFamily: fonts.text,
    },
    botaoeditar: {
        width: 150,
        backgroundColor: '#1C58F2',
        paddingVertical: 10,
        paddingHorizontal: 10,
        borderRadius: 10,
        alignItems: 'center',
        justifyContent: 'center',
        alignSelf: 'center',
        marginTop: 20,
    },
    textoeditar: {
        color: '#fff',
        fontSize: 14,
        fontFamily: fonts.text,
        fontWeight: 'bold',
    },
    dropdown: {
        width: 290,
        borderRadius: 6,
        borderWidth: 1,
        color:theme.text3,
        backgroundColor: theme.inputBackground,
        paddingVertical:8,    
        paddingHorizontal:20,
        marginBottom: 10,
        marginTop: 15,
    },  
    dropdownfuncionario: {
        width: 240,
        borderRadius: 6,
        borderWidth: 1,
        color:theme.text3,
        backgroundColor: theme.inputBackground,
        paddingVertical:8,    
        paddingHorizontal:20,
        marginBottom: 10,
        marginTop: 15,
    },  
    linha:{
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
    },
    botaoadd:{
        width:40,
        height: 40,
        borderRadius: 6,
        borderWidth: 1,
        borderBottomColor: '#D6D3D1',
        backgroundColor: '#1C58F2',
        justifyContent: 'center',
        alignItems: 'center',
        marginLeft: 10
    },
    imagemPreview: {
        padding:15,
        width:150,
        height:150,
        borderRadius:30,
        backgroundColor: theme.primary,
        alignItems: 'center',
        justifyContent: 'center',
    },

    modalContainer: {
        flex: 1,
        backgroundColor: 'rgba(0,0,0,0.5)', // fundo escuro transparente
        justifyContent: 'center',
        alignItems: 'center'
    },
    modalContent: {
        width: 300,
        backgroundColor: theme.background,
        borderRadius: 20,
        alignItems: 'center',
        elevation: 5 
    },
    nav2: {
        flexDirection: 'row',
        alignItems: 'flex-start',
        justifyContent: 'flex-start',   
        width: '100%',       
        padding: 15,
    },
    botaodevoltar: {
        width: 40,
        height: 40,
        justifyContent: 'center',
        zIndex: 1, 
    },
    textfoto: {
        color: theme.text,
        fontSize: 18,
        fontWeight: 'bold',
        fontFamily: fonts.text,
    },
    areafotototal:{
        padding:5,
    },
    areatitulofoto:{
        alignItems: 'flex-start',
        padding:10,
        paddingVertical:12,
    },
    buttonEnviar:{
        padding:15,
        width:150,
        height:150,
        backgroundColor: theme.primary,
        borderRadius:"25%",
        alignItems: 'center',
        justifyContent: 'center',
    },
    areafoto:{
        flexDirection: 'row',
        justifyContent: 'space-evenly',
        width: '100%',
        marginTop: 10,
        paddingHorizontal: 6,
        marginBottom:28
    },  
    button:{
        backgroundColor: theme.inputBackground,
        padding:5,
        marginBottom:25,
        borderRadius:5,
        width:"100%"
    },
    areafoto2:{
        marginTop:20,
        padding:5,
        alignItems: 'column', 
        gap: 3,
    },
    buttonText:{
        color: "#ffffff",
        fontSize: 13,
        fontFamily: fonts.text,
    },
    buttonText2:{
        color: theme.text,
        fontSize: 12,
        fontFamily: fonts.text,
    },
    tituloi: {
        width:150,
        height:50,
    },
    containerfuncionarios: {
        backgroundColor: theme.inputBackground,
        borderRadius: 10,
        padding: 10,
        marginBottom: 20,
        flexDirection: 'row',
        alignItems: 'center',
        width:300,
    },
    imag: {
        width: 35,
        height: 35,
        marginLeft: 10,
    },
});