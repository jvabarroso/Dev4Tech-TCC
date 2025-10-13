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
        marginTop:10,
    },
    botaodevoltar2: {
        width: 40,
        height: 40,
        justifyContent: 'center',
        zIndex: 1, 
    },
    titulo: {
        width:150,
        height:50,
    },
    titulo2: {
        fontSize: 18,
        color: theme.text,
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
        color: theme.text,
        marginBottom: 5,
    },
    datadeenvio: {
        fontSize: 13,
        fontFamily: fonts.text,
        color: theme.text2,
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
        color: theme.text2,
        marginBottom: 5,
    },
    datas: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: theme.text,
    },
    cargos: {
        borderRadius: 20,
        paddingHorizontal: 12,
        paddingVertical: 5,
    },
    textoCargo: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: theme.text,
    },
    linha2: {
        flexDirection: 'column',
        marginBottom: 5,
    },
    titulodescricao: {
        fontSize: 14,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        color: theme.text,
        marginBottom: 10,
    },
    descricao2: {
        fontSize: 15,
        fontFamily: fonts.text,
        color: theme.text2,
        marginBottom: 5,
        lineHeight: 20,
        width:300,
        textAlign:"left",
        paddingLeft:30,
        paddingRight:50,
        paddingVertical:5,
    },
    containerdescricao:{
        width:280,
        height:100,
        backgroundColor: theme.inputBackground,
        borderRadius: 6,
        borderWidth: 1,
        borderColor: '#D6D3D1',
    }, ///
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
        flexDirection:"row",
        alignItems: 'center'
    },
    botaoenviar: {
        backgroundColor: '#1C58F2',
        paddingVertical: 12,
        paddingHorizontal: 30,
        borderRadius: 10,
        alignItems: 'center',
        alignSelf: 'center',
        marginTop: 20,
        width:200,
    },
    textoenvio: {
        color: '#fff',
        fontSize: 16,
        fontFamily: fonts.text,
        fontWeight: 'bold',
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
        paddingVertical:10,
        bottom:15,
    },
    inputinstrucoes: {
        width:"80%",
        height: 100,
        fontSize: 16,
        borderRadius: 6,
        borderWidth: 1,
        borderBottomColor: '#D6D3D1',
        backgroundColor: theme.inputBackground,
        paddingVertical:8,    
        paddingHorizontal:25,
        marginBottom: 10,
        marginTop: 15,
        textAlignVertical: 'top'
    },
    texto: {
        fontSize: 16,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        color: theme.text3,
        alignSelf:"flex-start",
        paddingHorizontal:5,
    },
    textod:{
        color: theme.text,
    },
    textolistacargo: {
        color: theme.text,
        fontSize: 13,
        fontFamily: fonts.text,
        borderRadius: 15,
        paddingHorizontal: 5,
        paddingVertical: 3,
    },
    areaanexo:{
        padding:5
    },
    scrollDescricao: {
        maxHeight: 200,
    },



    modalOverlay: {
        flex: 1,
        backgroundColor: 'rgba(0,0,0,0.5)',
        justifyContent: 'center',
        alignItems: 'center',
        padding: 20,
    },
    modalContent: {
        backgroundColor: theme.inputBackground3,
        padding: 20,
        borderRadius: 12,
        width: '90%',
        maxHeight: '80%',
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.25,
        shadowRadius: 3.84,
        elevation: 5,
    },
    modalTitle: {
        fontSize: 18,
        fontWeight: 'bold',
        fontFamily: fonts.text,
        marginBottom: 15,
        textAlign: 'center',
        color: theme.text,
    },
    problemaTitle: {
        fontSize: 16,
        fontWeight: 'bold',
        fontFamily: fonts.text,
        marginBottom: 10,
        color: theme.text,
    },
    problemasScroll: {
        maxHeight: 200,
        marginBottom: 15,
    },
    problemaContainer: {
        backgroundColor: theme.mode === 'light' ? '#FFF3E0' : '#4E342E',
        padding: 12,
        borderRadius: 8,
        borderLeftWidth: 4,
        borderLeftColor: theme.mode === 'light' ? '#FF9800' : '#FFB74D',
        marginBottom: 10,
    },
    problemaIndex: {
        fontSize: 14,
        fontWeight: 'bold',
        fontFamily: fonts.text,
        marginBottom: 5,
        color: theme.mode === 'light' ? '#E65100' : '#FFCC80',
    },
    problemaText: {
        fontSize: 14,
        fontFamily: fonts.text,
        lineHeight: 18,
        color: theme.text,
    },
    instrucoesText: {
        fontSize: 14,
        fontFamily: fonts.text,
        color: theme.text2,
        marginBottom: 15,
        lineHeight: 20,
        textAlign: 'center',
    },
    textInput: {
        borderWidth: 1,
        borderColor: theme.border,
        borderRadius: 8,
        padding: 12,
        marginBottom: 15,
        textAlignVertical: 'top',
        minHeight: 100,
        fontSize: 14,
        fontFamily: fonts.text,
        backgroundColor: theme.inputBackground,
        color: theme.text,
    },
    modalButtons: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        marginTop: 10,
    },
    modalButton: {
        padding: 12,
        borderRadius: 6,
        minWidth: 120,
        alignItems: 'center',
    },
    modalButtonCancel: {
        backgroundColor: theme.inputBackground2,
    },
    modalButtonConfirm: {
        backgroundColor: theme.primary,
    },
    modalButtonTextCancel: {
        fontWeight: 'bold',
        fontSize: 14,
        fontFamily: fonts.text,
        color: theme.text,
    },
    modalButtonTextConfirm: {
        fontWeight: 'bold',
        fontSize: 14,
        fontFamily: fonts.text,
        color: "#ffff",
    },
});