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
        width:"90%",
        borderRadius: 6,
        borderWidth: 1,
        borderBottomColor: '#D6D3D1',
        color:theme.text2,
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
        borderBottomColor: '#D6D3D1',
        color:theme.text2,
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
        borderBottomColor: '#D6D3D1',
        color:theme.text2,
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
    }
});