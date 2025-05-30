import { StyleSheet } from "react-native";
import fonts from "../../styles/fonts";



export const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: "#ffffff",
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
    nav2: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
        paddingVertical: 20,
        marginBottom: 10,
    },
    botaodevoltar: {
        width: 40,
        height: 40,
        justifyContent: 'center',
    },
    botaodevoltar2: {
        width: 40,
        height: 40,
        justifyContent: 'center',
        zIndex: 1, 
    },
    titulo: {
        fontSize: 18,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        textAlign: 'center',
        flex: 1,
    },
    titulo2: {
        fontSize: 18,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        textAlign: 'right',
        flex: 1,
    },
    espacoHeader: {
        width: 40,
    },
    
    areadetalhes: {
        flex: 1,
    },
    imagem: {
        width: 80,
        height: 80,
        borderRadius: 8,
        marginBottom: 15,
    },
    titulotarefa: {
        fontSize: 24,
        fontWeight: 'bold',
        fontFamily: fonts.text,
        color: "#000000",
        marginBottom: 5,
    },
    datadeenvio: {
        fontSize: 13,
        fontFamily: fonts.text,
        color: "#aaaaaa",
        marginBottom: 20,
    },
    
    linha: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        marginBottom: 20,
    },
    coluna: {
        flex: 1,
        marginRight: 10,
    },
    colunaEquipe: {
        flex: 1,
    },
    subtitulos: {
        fontSize: 13,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        color: "#181A1F",
        marginBottom: 5,
    },
    datas: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: '#000',
    },
    cargos: {
        backgroundColor: '#F5F7FC',
        borderRadius: 20,
        paddingHorizontal: 12,
        paddingVertical: 5,
        alignSelf: 'flex-start',
    },
    textoCargo: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: '#181A1F',
    },
    
    linha2: {
        flexDirection: 'column',
        marginBottom: 25,
    },
    titulodescricao: {
        fontSize: 14,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        color: '#000',
        marginBottom: 10,
    },
    descricao2: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: '#333',
        marginBottom: 5,
        lineHeight: 20,
    },
    textodescr: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: '#1C58F2',
        fontWeight: 'bold',
    },
    
    botaomostrar: {
        paddingVertical: 8,
    },
    textoadd: {
        color: "#1C58F2",
        fontWeight: 'bold',
        fontFamily: fonts.text,
        fontSize: 14,
    },
    textoproblem: {
        color: "#F21C1C",
        fontWeight: 'bold',
        fontFamily: fonts.text,
        fontSize: 14,
    },
    
    botaoenviar: {
        backgroundColor: '#1C58F2',
        paddingVertical: 12,
        paddingHorizontal: 30,
        borderRadius: 10,
        alignItems: 'center',
        alignSelf: 'center',
        marginTop: 20,
    },
    textoenvio: {
        color: '#fff',
        fontSize: 16,
        fontFamily: fonts.text,
        fontWeight: 'bold',
    },
    modalContainer: {
        flex: 1,
        backgroundColor: '#ffffff',
    },
    modalContent: {
        flex: 1,
        padding: 20,     
    },
    modalMainContent: {
        flex: 1,
        justifyContent: 'flex-start', 
    },
    containermensagem:{
        marginTop: 5,
        justifyContent: 'center',
    },
    mensagem:{
        padding:20,
        backgroundColor:"#EEEEEE",
        borderRadius:30,
        borderBottomLeftRadius:1,
        maxWidth: '80%',
        zIndex: 2
    },
    modeltexto:{
        fontSize:16,
        fontFamily: fonts.text,
        color:"#000000"
    },
    espacoInput: {
        flex: 1, 
    },
    containerinput: {
        position: 'relative',
        borderWidth: 1,
        borderColor: '#ccc',
        borderRadius:60,
        height: 50,
        flexDirection: 'row',
        alignItems: 'center',
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 }, 
        shadowOpacity: 0.25,              
        shadowRadius: 3.84,              
        elevation: 5,                 
    },
    textInput: {
        paddingVertical: 2, 
        paddingHorizontal:40,
        width:290,
        height: 35,   
        fontSize: 14,  
        fontFamily: fonts.text,
        marginTop:10,
    },
    botaoEnviar: {
        marginTop:5,
    },
    imagemfundo:{
        position: 'absolute',
        resizeMode: 'cover',
        alignSelf:"center",
        opacity: 0.5,
        bottom:180,
        zIndex: 1
    },
    mensagemEnviada: {
        marginLeft:60,
        backgroundColor: "#1C58F2",
        borderBottomLeftRadius: 20,
        borderBottomRightRadius: 1,
    },
    problem:{
        marginBottom:10,
        bottom:23,
    },
});