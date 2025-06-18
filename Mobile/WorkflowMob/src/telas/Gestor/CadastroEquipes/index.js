import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { styles } from './style';

import { Ionicons } from '@expo/vector-icons';

export default function CadastroEquipes({navigation}){

    const [funcionario, setFuncionario] = useState([
    {
        id: '1',
        nome: 'Gabriel Kenzo Takeuchi',
        datadenascimento: "16/05/1980",
        email: 'kenzo@empresa.com',
        telefone: 13982176670,
        endereco: "Rua João da Fonseca, 123 - Jardim Mato Grosso, Cananeia/SP",
        cargo: "Desenvolvedor Web Sênior",
        equipe: null,
        senha: '1234',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '2',
        nome: 'Leonardo Silva',
        datadenascimento: "22/11/1992",
        email: 'leonardo.silva@empresa.com',
        telefone: 11987654321,
        endereco: "Av. Paulista, 1000 - São Paulo/SP",
        cargo: "Designer UX/UI",
        equipe: null,
        senha: 'abcd123',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '3',
        nome: 'Ana Carolina Oliveira',
        datadenascimento: "05/03/1985",
        email: 'ana.oliveira@empresa.com',
        telefone: 21999887766,
        endereco: "Rua das Flores, 45 - Rio de Janeiro/RJ",
        cargo: "Gerente de Projetos",
        equipe: null,
        senha: 'ana2023',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '4',
        nome: 'Carlos Eduardo Santos',
        datadenascimento: "30/07/1990",
        email: 'carlos.santos@empresa.com',
        telefone: 31988776655,
        endereco: "Av. Afonso Pena, 2000 - Belo Horizonte/MG",
        cargo: "Analista de Dados",
        equipe: null,
        senha: 'carlos123',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '5',
        nome: 'Mariana Costa',
        datadenascimento: "14/09/1988",
        email: 'mariana.costa@empresa.com',
        telefone: 41977665544,
        endereco: "Rua XV de Novembro, 500 - Curitiba/PR",
        cargo: "Product Owner",
        equipe: null,
        senha: 'mari@2023',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '6',
        nome: 'Rafael Pereira',
        datadenascimento: "03/12/1995",
        email: 'rafael.pereira@empresa.com',
        telefone: 51966554433,
        endereco: "Av. Borges de Medeiros, 300 - Porto Alegre/RS",
        cargo: "Desenvolvedor Mobile",
        equipe: null,
        senha: 'rafa1234',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    }
    ]);
    
    const [equipe, setEquipe] = useState([
        {
          id: '1',
          titulo: 'Equipe 1',
          cargo: 'Desenvolvimento de Software',
          tarefaspostadas: 20,
          quantdeproblemas:6,
          tarefasatrasadas:1,
          tarefasnaoentregues: 6,
          imagem: require('../../../../assets/img/image.png'),
        },
        {
          id: '2',
          titulo: 'Equipe 2',
          cargo: 'Design',
          tarefaspostadas: 10,
          quantdeproblemas:2,
          tarefasatrasadas:2,
          tarefasnaoentregues: 2,
          imagem: require('../../../../assets/img/image.png'),
        }
      ]);
      
    const [funcionarios, setFuncionarios] = useState(true);
    const [funcionarioselecionada, setfuncionariosselecionada] = useState(null);
    const [equipes, setequipes] = useState(true);
    const [equipeselecionada, setEquipeselecionada] = useState(null);

    const cliqueinformacao = () => 
        {
            setFuncionarios(valorAtual => !valorAtual); 
        };
    
    const confirmodeequipe = (funcionarioselecionada) => 
        {   
            setfuncionariosselecionada(funcionarioselecionada.nome);
            setFuncionarios(valorAtual => !valorAtual); 
        };

    const cliquefuncionario = (funcionarioselecionada,equipeselecionada) => 
        {
            if(funcionarioselecionada.equipe == null){
                setfuncionariosselecionada(equipeselecionada.titulo);
                setequipes(valorAtual => !valorAtual); 

            }
        };
    

    
    return(
        <View style={styles.container}>
            <ScrollView contentContainerStyle={styles.scrollContent}>
                <Text style={styles.titulo}>Criar uma equipe</Text>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Nome da Equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite o nome da equipe"
                        placeholderTextColor={"#000000"}
                    />
                    <Text style={styles.texto}>Categoria da equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite a categoria da equipe"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    <Text style={styles.texto}>Adicionar membros à equipe</Text>
                    <View style={styles.linha}>
                        <TouchableOpacity
                            style={styles.inputequipe}
                            onPress={cliqueinformacao}
                        >
                            <Text style={styles.textobotao}>{funcionarioselecionada || "Selecione uma funcionario"}</Text> 
                            <Ionicons name="caret-down-outline" size={18} color="black" />
                        </TouchableOpacity>

                        <TouchableOpacity
                            style={styles.inputadicionar}
                            onPress={cliquefuncionario}
                        >
                            <Text style={styles.textobotao2}>+</Text> 
                        </TouchableOpacity>
                    </View>

                    {!funcionarios && funcionario.map(item => (
                        <TouchableOpacity
                            key={item.id}
                            style={styles.containerequipes}
                            onPress={() => confirmodeequipe(item)}
                        >
                        <Image source={item.imagem} style={styles.imag} />
                        <View style={styles.textos}>
                            <Text style={styles.textolistatitulo}>{item.nome}</Text>
                            <Text style={styles.textolistacargo}>{item.cargo}</Text>
                        </View>
                        </TouchableOpacity>
                    ))}

                    <Text style={styles.texto}>Membros da Equipe</Text>

                    {!equipes && funcionario.map(item => (
                        <TouchableOpacity
                            key={item.id}
                            style={styles.containerequipes}
                            onPress={() => confirmodeequipe(item)}
                        >
                        <Image source={item.imagem} style={styles.imag} />
                        <View style={styles.textos}>
                            <Text style={styles.textolistatitulo}>{item.nome}</Text>
                            <Text style={styles.textolistatitulo}>{item.equipe}</Text>
                            <Text style={styles.textolistacargo}>{item.cargo}</Text>
                        </View>
                        </TouchableOpacity>
                    ))}

                    <TouchableOpacity
                        style={styles.inputfuncionario}
                        onPress={cliquefuncionario}
                    >
                        <Text style={styles.textobotao3}>Ver Equipe</Text> 
                    </TouchableOpacity>

                    <TouchableOpacity style={styles.botaocriar}>
                        <Text style={styles.textocriar}>Criar</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    )
}
